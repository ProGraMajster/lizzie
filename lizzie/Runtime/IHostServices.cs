using System;

namespace lizzie.Runtime
{
    /// <summary>
    /// Provides access to host services such as randomness and clocks.
    /// </summary>
    public interface IHostServices
    {
        /// <summary>
        /// Deterministic random number generator.
        /// </summary>
        Random Random { get; }

        /// <summary>
        /// Clock abstraction used for deterministic time.
        /// </summary>
        IClock Clock { get; }
    }
}
