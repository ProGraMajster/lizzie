using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using lizzie.Runtime;
using lizzie.Runtime.Config;
using lizzie.Std;
using Xunit;
using System.IO;

namespace lizzie.tests
{
    public class RuntimeFeaturesTests
    {
        [Fact]
        public void ExecutionTimeoutEnforced()
        {
            var limiter = new DefaultResourceLimiter(timeout: TimeSpan.FromMilliseconds(10));
            Thread.Sleep(20);
            Assert.Throws<TimeoutException>(() => limiter.Tick());
        }

        [Fact]
        public void DeterministicRngWithSeed()
        {
            var limiter = new DefaultResourceLimiter();
            Rand.seed(123, limiter);
            var expected = new Random(123).Next(0, 100);
            var actual = Rand.nextInt(0, 100, limiter);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ModuleLoadingAndCaching()
        {
            var runtime = new DefaultRuntime();
            var code = "line1\nline2";
            var module = runtime.Compile(code, "test");
            using var sha = SHA256.Create();
            var hash = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(code)));
            var cacheField = typeof(DefaultRuntime).GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var cache = (InMemoryModuleCache)cacheField.GetValue(runtime)!;
            Assert.True(cache.TryGet(hash, "test", out var cached));
            Assert.Same(module, cached);
        }

        [Fact]
        public void InstructionCountingByDefaultResourceLimiter()
        {
            var limiter = new DefaultResourceLimiter(maxInstructions: 3);
            limiter.Tick();
            limiter.Tick();
            limiter.Tick();
            Assert.Throws<InvalidOperationException>(() => limiter.Tick());
        }

        private class SandboxLimiter : IResourceLimiter
        {
            private readonly ISandboxPolicy _sandbox;
            public SandboxLimiter(ISandboxPolicy sandbox) { _sandbox = sandbox; }
            public void Enter() { }
            public void Exit() { }
            public void Tick() { }
            public void Demand(Capability capability)
            {
                if (!_sandbox.Has(capability))
                    throw new InvalidOperationException("Capability missing");
            }
        }

        [Fact]
        public void CapabilityChecksBlockUnauthorizedStdlibCalls()
        {
            var sandbox = new CapabilitySandbox();
            var limiter = new SandboxLimiter(sandbox);
            Assert.Throws<InvalidOperationException>(() => Time.now(limiter));
        }

        [Fact]
        public void ServerDefaultsAllowFileSystem()
        {
            var ctx = RuntimeProfiles.ServerDefaults();
            Assert.True(ctx.Sandbox.Has(Capability.FileSystem));
        }

        [Fact]
        public void UnityDefaultsDenyFileSystem()
        {
            var ctx = RuntimeProfiles.UnityDefaults();
            Assert.False(ctx.Sandbox.Has(Capability.FileSystem));
        }

        [Fact]
        public void FilesystemWhitelistEnforced()
        {
            var temp = Path.GetTempPath();
            var ctx = RuntimeProfiles.ServerDefaults(filesystemWhitelist: new[] { temp });
            var fsPolicy = Assert.IsAssignableFrom<IFilesystemPolicy>(ctx.Sandbox);
            var allowed = Path.Combine(temp, "file.txt");
            var disallowed = Path.Combine(Path.GetPathRoot(temp) ?? "/", "somewhere", "else.txt");
            Assert.True(fsPolicy.IsPathAllowed(allowed));
            Assert.False(fsPolicy.IsPathAllowed(disallowed));
        }

        [Fact]
        public void HttpWhitelistEnforced()
        {
            var ctx = RuntimeProfiles.ServerDefaults(httpWhitelist: new[] { "https://example.com" });
            Assert.True(ctx.Sandbox.Has(Capability.Network));
            var netPolicy = Assert.IsAssignableFrom<INetworkPolicy>(ctx.Sandbox);
            Assert.True(netPolicy.IsOriginAllowed(new Uri("https://example.com/resource")));
            Assert.False(netPolicy.IsOriginAllowed(new Uri("https://notallowed.com")));
        }

        [Fact]
        public void NetworkCapabilityMissingWithoutWhitelist()
        {
            var ctx = RuntimeProfiles.ServerDefaults();
            Assert.False(ctx.Sandbox.Has(Capability.Network));
        }
    }
}
