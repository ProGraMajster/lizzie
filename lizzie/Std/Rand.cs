using System;
using lizzie.Runtime;

namespace lizzie.Std
{
    public static class Rand
    {
        private static Random _rng = new();

        /// <summary>
        /// Seeds the internal pseudo random number generator.
        /// </summary>
        public static void seed(int n, IResourceLimiter limiter)
        {
            limiter.Demand(Capability.Random);
            _rng = new Random(n);
        }

        /// <summary>
        /// Returns a floating point value in the range [0,1).
        /// </summary>
        public static double nextFloat(IResourceLimiter limiter)
        {
            limiter.Demand(Capability.Random);
            return _rng.NextDouble();
        }

        /// <summary>
        /// Returns an integer between the supplied bounds.
        /// </summary>
        public static int nextInt(int min, int max, IResourceLimiter limiter)
        {
            limiter.Demand(Capability.Random);
            return _rng.Next(min, max);
        }
    }
}
