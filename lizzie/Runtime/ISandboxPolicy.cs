namespace lizzie.Runtime
{
    /// <summary>
    /// Policy controlling which capabilities are available to a script.
    /// </summary>
    public interface ISandboxPolicy
    {
        /// <summary>
        /// Checks if the specified capability has been granted.
        /// </summary>
        bool Has(Capability capability);

        /// <summary>
        /// Grants the specified capability.
        /// </summary>
        void Allow(Capability capability);

        /// <summary>
        /// Revokes the specified capability.
        /// </summary>
        void Deny(Capability capability);
    }
}
