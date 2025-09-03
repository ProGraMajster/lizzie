namespace lizzie.Runtime
{
    /// <summary>
    /// Represents the environment a script executes within.
    /// </summary>
    public interface IScriptContext
    {
        IScheduler Scheduler { get; }
        ISandboxPolicy Sandbox { get; }
        IBindingRegistry Bindings { get; }
        IResourceLimiter Resources { get; }
        IHostServices Host { get; }
    }
}
