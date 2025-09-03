namespace lizzie.Runtime
{
    public interface IResourceLimiter
    {
        /// <summary>
        /// Called when entering a new execution frame. Implementations can
        /// use this to track recursion depth.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called when leaving an execution frame.
        /// </summary>
        void Exit();

        /// <summary>
        /// Signals that a single instruction has executed.
        /// </summary>
        void Tick();

        /// <summary>
        /// Ensures the caller has been granted the specified capability.
        /// Implementations should throw if the capability is not available
        /// or resources have been exhausted.
        /// </summary>
        /// <param name="capability">The capability required by the caller.</param>
        void Demand(Capability capability);
    }
}
