using System;
using System.Threading.Tasks;

namespace lizzie.Runtime
{
    /// <summary>
    /// Scheduler responsible for micro task execution and timing features.
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Queues a piece of work to be executed as a "micro task".
        /// </summary>
        /// <param name="task">The work to execute.</param>
        void QueueMicrotask(Func<Task> task);

        /// <summary>
        /// Introduces an asynchronous delay.
        /// </summary>
        /// <param name="delay">Amount of time to delay.</param>
        Task Delay(TimeSpan delay);

        /// <summary>
        /// Drains the micro task queue.
        /// </summary>
        Task DrainAsync();
    }
}
