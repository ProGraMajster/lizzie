using System;

namespace lizzie.Runtime
{
    /// <summary>
    /// Policy controlling which network origins are allowed for requests.
    /// </summary>
    public interface INetworkPolicy
    {
        /// <summary>
        /// Determines whether the specified origin is permitted.
        /// </summary>
        bool IsOriginAllowed(Uri origin);
    }
}
