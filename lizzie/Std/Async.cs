using System;
using System.Threading.Tasks;
using lizzie.Runtime;

namespace lizzie.Std
{
    public static class Async
    {
        /// <summary>
        /// Queues the provided callback to execute asynchronously on the thread pool.
        /// </summary>
        public static Task nextTick(Action fn, IResourceLimiter limiter)
        {
            limiter.Demand(Capability.Async);
            return Task.Run(fn);
        }
    }
}
