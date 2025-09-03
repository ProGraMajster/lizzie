namespace lizzie.Runtime
{
    public interface IResourceLimiter
    {
        /// <summary>
        /// Ensures the caller has been granted the specified capability.
        /// Implementations should throw if the capability is not available
        /// or resources have been exhausted.
        /// </summary>
        /// <param name="capability">The capability required by the caller.</param>
        void Demand(Capability capability);
    }
}
