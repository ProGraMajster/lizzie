using System;
using System.IO;
using lizzie.Runtime;
using FileModule = lizzie.Std.File;
using Xunit;

namespace lizzie.tests
{
    public class FileSystemCapabilityTests
    {
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
        public void FileOperationsSucceedWhenCapabilityAllowedAndPathWhitelisted()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, "a.txt");
            System.IO.File.WriteAllText(file, "allowed");
            try
            {
                var ctx = RuntimeProfiles.ServerDefaults(filesystemWhitelist: new[] { dir });
                var limiter = new SandboxLimiter(ctx.Sandbox);
                var policy = Assert.IsAssignableFrom<IFilesystemPolicy>(ctx.Sandbox);
                var content = FileModule.readFile(file, limiter, policy);
                Assert.Equal("allowed", content);
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Fact]
        public void FileOperationsDeniedWhenCapabilityMissing()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, "a.txt");
            System.IO.File.WriteAllText(file, "data");
            try
            {
                var ctx = RuntimeProfiles.ServerDefaults(filesystemWhitelist: new[] { dir });
                ctx.Sandbox.Deny(Capability.FileSystem);
                var limiter = new SandboxLimiter(ctx.Sandbox);
                var policy = Assert.IsAssignableFrom<IFilesystemPolicy>(ctx.Sandbox);
                Assert.Throws<InvalidOperationException>(() => FileModule.readFile(file, limiter, policy));
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Fact]
        public void FileOperationsDeniedWhenPathOutsideWhitelist()
        {
            var allowedDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(allowedDir);
            var disallowedDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(disallowedDir);
            var file = Path.Combine(disallowedDir, "a.txt");
            System.IO.File.WriteAllText(file, "nope");
            try
            {
                var ctx = RuntimeProfiles.ServerDefaults(filesystemWhitelist: new[] { allowedDir });
                var limiter = new SandboxLimiter(ctx.Sandbox);
                var policy = Assert.IsAssignableFrom<IFilesystemPolicy>(ctx.Sandbox);
                Assert.Throws<UnauthorizedAccessException>(() => FileModule.readFile(file, limiter, policy));
            }
            finally
            {
                Directory.Delete(allowedDir, true);
                Directory.Delete(disallowedDir, true);
            }
        }
    }
}
