namespace lizzie.Runtime
{
    /// <summary>
    /// Sandbox policy based on a set of capabilities.
    /// </summary>
    public class CapabilitySandbox : ISandboxPolicy
    {
        private Capability _capabilities;

        public CapabilitySandbox(Capability initial = Capability.None)
        {
            _capabilities = initial;
        }

        public bool Has(Capability capability) => (_capabilities & capability) == capability;

        public void Allow(Capability capability)
        {
            _capabilities |= capability;
        }

        public void Deny(Capability capability)
        {
            _capabilities &= ~capability;
        }
    }
}
