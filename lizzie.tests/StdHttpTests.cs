using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using lizzie.Runtime;
using HttpModule = lizzie.Std.Http;
using Xunit;

namespace lizzie.tests
{
    public class StdHttpTests
    {
        private class TestLimiter : IResourceLimiter
        {
            public Capability? Requested { get; private set; }
            public void Enter() { }
            public void Exit() { }
            public void Tick() { }
            public void Demand(Capability capability) => Requested = capability;
        }

        private class TestPolicy : INetworkPolicy
        {
            private readonly bool _allow;
            public TestPolicy(bool allow) { _allow = allow; }
            public bool IsOriginAllowed(Uri origin) => _allow;
        }

        private static int GetFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        [Fact]
        public void GetDemandsCapabilityAndRespectsPolicy()
        {
            var port = GetFreePort();
            var url = $"http://localhost:{port}/";
            using var listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            var handler = Task.Run(async () =>
            {
                var ctx = await listener.GetContextAsync();
                using var writer = new StreamWriter(ctx.Response.OutputStream);
                await writer.WriteAsync("hello");
                writer.Flush();
                ctx.Response.Close();
            });

            var limiter = new TestLimiter();
            var policy = new TestPolicy(true);
            var content = HttpModule.get(url, limiter, policy);
            handler.Wait();
            listener.Stop();

            Assert.Equal("hello", content);
            Assert.Equal(Capability.Network, limiter.Requested);
        }

        [Fact]
        public void PostDemandsCapabilityAndRespectsPolicy()
        {
            var port = GetFreePort();
            var url = $"http://localhost:{port}/";
            using var listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            var handler = Task.Run(async () =>
            {
                var ctx = await listener.GetContextAsync();
                using var reader = new StreamReader(ctx.Request.InputStream);
                var received = await reader.ReadToEndAsync();
                var buffer = Encoding.UTF8.GetBytes(received);
                ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
                ctx.Response.Close();
            });

            var limiter = new TestLimiter();
            var policy = new TestPolicy(true);
            var result = HttpModule.post(url, "payload", limiter, policy);
            handler.Wait();
            listener.Stop();

            Assert.Equal("payload", result);
            Assert.Equal(Capability.Network, limiter.Requested);
        }

        [Fact]
        public void GetDeniedWhenPolicyRejects()
        {
            var limiter = new TestLimiter();
            var policy = new TestPolicy(false);
            Assert.Throws<UnauthorizedAccessException>(() => HttpModule.get("http://localhost/", limiter, policy));
            Assert.Equal(Capability.Network, limiter.Requested);
        }

        [Fact]
        public void BindingRegistryProvidesGetAndPost()
        {
            var ctx = RuntimeProfiles.ServerDefaults(httpWhitelist: new[] { "https://example.com" });
            Assert.True(ctx.Bindings.TryGet("get", out var getFn));
            Assert.IsType<Func<string, string>>(getFn);
            Assert.True(ctx.Bindings.TryGet("post", out var postFn));
            Assert.IsType<Func<string, string, string>>(postFn);
        }
    }
}
