using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace lizzie.Runtime
{
    /// <summary>
    /// Collection of helpers for constructing commonly used runtime
    /// configurations.
    /// </summary>
    public static class RuntimeProfiles
    {
        /// <summary>
        /// Creates a <see cref="IScriptContext"/> with settings suited for the
        /// Unity environment. The returned context enables the
        /// <see cref="Capability.UnityMainThread"/> capability and installs a
        /// resource limiter that enforces a simple per-frame instruction
        /// budget.
        /// </summary>
        /// <param name="frameBudget">Maximum number of instructions allowed per frame.</param>
        public static IScriptContext UnityDefaults(int frameBudget = 1000)
        {
            var sandbox = new CapabilitySandbox(Capability.UnityMainThread | Capability.Time | Capability.Async | Capability.Random);
            var limiter = new FrameBudgetLimiter(frameBudget);
            var ctx = new DefaultScriptContext(
                scheduler: new DefaultScheduler(),
                sandbox: sandbox,
                bindings: new SimpleBindingRegistry(),
                resources: limiter,
                host: new DefaultHostServices());
            ctx.BindFileModule();
            ctx.BindHttpModule();
            return ctx;
        }

        /// <summary>
        /// Creates a <see cref="IScriptContext"/> configured for a typical
        /// server environment. The context omits the
        /// <see cref="Capability.UnityMainThread"/> capability and exposes
        /// read-only HTTP and filesystem whitelists.
        /// </summary>
        /// <param name="httpWhitelist">Collection of HTTP origins allowed for requests.</param>
        /// <param name="filesystemWhitelist">Collection of directory paths allowed for file access.</param>
        public static IScriptContext ServerDefaults(
            IEnumerable<string>? httpWhitelist = null,
            IEnumerable<string>? filesystemWhitelist = null)
        {
            var sandbox = new ReadOnlySandboxPolicy(httpWhitelist, filesystemWhitelist);
            sandbox.Allow(Capability.Time);
            sandbox.Allow(Capability.Async);
            sandbox.Allow(Capability.Random);
            sandbox.Allow(Capability.FileSystem);
            if (httpWhitelist != null)
                sandbox.Allow(Capability.Network);
            var limiter = new DefaultResourceLimiter();
            var ctx = new DefaultScriptContext(
                scheduler: new DefaultScheduler(),
                sandbox: sandbox,
                bindings: new SimpleBindingRegistry(),
                resources: limiter,
                host: new DefaultHostServices());
            ctx.BindFileModule();
            ctx.BindHttpModule();
            return ctx;
        }

        /// <summary>
        /// Resource limiter enforcing a simple instruction budget for each
        /// execution frame. The budget is reset when entering a new frame.
        /// </summary>
        private sealed class FrameBudgetLimiter : IResourceLimiter
        {
            private readonly int _budget;
            private int _executed;

            public FrameBudgetLimiter(int budget)
            {
                _budget = budget;
            }

            public void Enter()
            {
                _executed = 0;
            }

            public void Exit()
            {
            }

            public void Tick()
            {
                _executed++;
                if (_budget > 0 && _executed > _budget)
                    throw new InvalidOperationException("Frame budget exceeded");
            }

            public void Demand(Capability capability)
            {
                // No capability tracking required for this limiter.
            }
        }

        /// <summary>
        /// Sandbox policy that stores read-only HTTP and filesystem whitelists
        /// while delegating capability management to an internal
        /// <see cref="CapabilitySandbox"/>.
        /// </summary>
        private sealed class ReadOnlySandboxPolicy : ISandboxPolicy, IFilesystemPolicy, INetworkPolicy
        {
            private readonly CapabilitySandbox _capabilities = new();

            public IReadOnlyCollection<string> HttpWhitelist { get; }
            public IReadOnlyCollection<string> FilesystemWhitelist { get; }

            public ReadOnlySandboxPolicy(
                IEnumerable<string>? httpWhitelist,
                IEnumerable<string>? filesystemWhitelist)
            {
                HttpWhitelist = new HashSet<string>(httpWhitelist ?? Enumerable.Empty<string>());
                FilesystemWhitelist = new HashSet<string>(filesystemWhitelist ?? Enumerable.Empty<string>());
            }

            public bool Has(Capability capability) => _capabilities.Has(capability);
            public void Allow(Capability capability) => _capabilities.Allow(capability);
            public void Deny(Capability capability) => _capabilities.Deny(capability);

            public bool IsPathAllowed(string path)
            {
                var full = Path.GetFullPath(path);
                foreach (var allowed in FilesystemWhitelist)
                {
                    var allowedFull = Path.GetFullPath(allowed)
                        .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    if (full.StartsWith(allowedFull + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(full, allowedFull, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }

            public bool IsOriginAllowed(Uri origin)
            {
                var auth = origin.GetLeftPart(UriPartial.Authority);
                foreach (var allowed in HttpWhitelist)
                {
                    if (string.Equals(auth, allowed, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }
        }
    }
}
