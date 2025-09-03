namespace lizzie.Runtime
{
    /// <summary>
    /// Sandbox policy that permits all capabilities.
    /// </summary>
    public class NoopSandboxPolicy : ISandboxPolicy
    {
        public bool Has(Capability capability) => true;
        public void Allow(Capability capability) { }
        public void Deny(Capability capability) { }
    }
}
