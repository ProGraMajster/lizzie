namespace lizzie.Runtime
{
    /// <summary>
    /// Default implementation of <see cref="IScriptContext"/> assembling the various runtime services.
    /// </summary>
    public class DefaultScriptContext : IScriptContext
    {
        public IScheduler Scheduler { get; }
        public ISandboxPolicy Sandbox { get; }
        public IBindingRegistry Bindings { get; }
        public IResourceLimiter Resources { get; }
        public IHostServices Host { get; }

        public DefaultScriptContext(
            IScheduler? scheduler = null,
            ISandboxPolicy? sandbox = null,
            IBindingRegistry? bindings = null,
            IResourceLimiter? resources = null,
            IHostServices? host = null)
        {
            Scheduler = scheduler ?? new DefaultScheduler();
            Sandbox = sandbox ?? new NoopSandboxPolicy();
            Bindings = bindings ?? new SimpleBindingRegistry();
            Resources = resources ?? new DefaultResourceLimiter();
            Host = host ?? new DefaultHostServices();
        }
    }
}
