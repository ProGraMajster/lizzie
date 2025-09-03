using System;
using System.Threading.Tasks;
using lizzie.Runtime;

namespace lizzie.Std
{
    public static class Time
    {
        /// <summary>
        /// Returns the current time in milliseconds since the Unix epoch.
        /// </summary>
        public static double now(IResourceLimiter limiter)
        {
            limiter.Demand(Capability.Time);
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Asynchronously sleeps for the specified number of milliseconds.
        /// </summary>
        public static Task sleep(int ms, IResourceLimiter limiter)
        {
            limiter.Demand(Capability.Time);
            return Task.Delay(ms);
        }
    }
}
