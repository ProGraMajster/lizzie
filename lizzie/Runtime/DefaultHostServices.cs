using System;

namespace lizzie.Runtime
{
    /// <summary>
    /// Default implementation of host services providing a deterministic RNG and clock.
    /// </summary>
    public class DefaultHostServices : IHostServices
    {
        private class SystemClock : IClock
        {
            public DateTime UtcNow => DateTime.UtcNow;
        }

        public Random Random { get; }
        public IClock Clock { get; }

        public DefaultHostServices(int? seed = null, IClock? clock = null)
        {
            Random = seed.HasValue ? new Random(seed.Value) : new Random();
            Clock = clock ?? new SystemClock();
        }
    }
}
