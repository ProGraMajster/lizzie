using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lizzie.Runtime
{
    /// <summary>
    /// Simple scheduler implementing a micro task queue and delay feature.
    /// </summary>
    public class DefaultScheduler : IScheduler
    {
        private readonly Queue<Func<Task>> _microTasks = new();

        public void QueueMicrotask(Func<Task> task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            lock (_microTasks)
            {
                _microTasks.Enqueue(task);
            }
        }

        public Task Delay(TimeSpan delay)
        {
            return Task.Delay(delay);
        }

        public async Task DrainAsync()
        {
            while (true)
            {
                Func<Task>? next = null;
                lock (_microTasks)
                {
                    if (_microTasks.Count > 0)
                        next = _microTasks.Dequeue();
                }

                if (next == null)
                    break;

                await next();
            }
        }
    }
}
