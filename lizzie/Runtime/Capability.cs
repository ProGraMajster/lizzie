using System;

namespace lizzie.Runtime
{
    [Flags]
    public enum Capability
    {
        None = 0,
        Time = 1 << 0,
        Async = 1 << 1,
        Random = 1 << 2
    }
}
