using System;
using System.IO;
using lizzie.Runtime;
using FileModule = lizzie.Std.File;
using Xunit;

namespace lizzie.tests
{
    public class StdFileTests
    {
        private class TestLimiter : IResourceLimiter
        {
            public Capability? Requested { get; private set; }
            public void Enter() { }
            public void Exit() { }
            public void Tick() { }
            public void Demand(Capability capability) => Requested = capability;
        }

        private class TestPolicy : IFilesystemPolicy
        {
            private readonly bool _allow;
            public TestPolicy(bool allow) { _allow = allow; }
            public bool IsPathAllowed(string path) => _allow;
        }

        [Fact]
        public void ReadFileDemandsCapabilityAndRespectsPolicy()
        {
            var temp = Path.GetTempFileName();
            System.IO.File.WriteAllText(temp, "hello");
            var limiter = new TestLimiter();
            var policy = new TestPolicy(true);
            var content = FileModule.readFile(temp, limiter, policy);
            Assert.Equal("hello", content);
            Assert.Equal(Capability.FileSystem, limiter.Requested);
        }

        [Fact]
        public void ReadFileDeniedWhenPolicyRejects()
        {
            var temp = Path.GetTempFileName();
            System.IO.File.WriteAllText(temp, "hello");
            var limiter = new TestLimiter();
            var policy = new TestPolicy(false);
            Assert.Throws<UnauthorizedAccessException>(() => FileModule.readFile(temp, limiter, policy));
        }

        [Fact]
        public void BindingRegistryProvidesReadFile()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, "a.txt");
            System.IO.File.WriteAllText(file, "data");
            var ctx = RuntimeProfiles.ServerDefaults(filesystemWhitelist: new[] { dir });
            Assert.True(ctx.Bindings.TryGet("readFile", out var fn));
            var reader = Assert.IsType<Func<string, string>>(fn);
            var content = reader(file);
            Assert.Equal("data", content);
        }
    }
}
