using System;
using System.Diagnostics;

namespace lizzie.Runtime
{
    /// <summary>
    /// Resource limiter enforcing instruction count, timeout and call depth.
    /// </summary>
    public class DefaultResourceLimiter : IResourceLimiter
    {
        private readonly int _maxInstructions;
        private readonly TimeSpan? _timeout;
        private readonly int _maxDepth;
        private readonly Stopwatch _stopwatch = new();
        private int _instructions;
        private int _depth;

        public DefaultResourceLimiter(int maxInstructions = 0, TimeSpan? timeout = null, int maxDepth = int.MaxValue)
        {
            _maxInstructions = maxInstructions;
            _timeout = timeout;
            _maxDepth = maxDepth;
            if (timeout.HasValue)
                _stopwatch.Start();
        }

        public void Enter()
        {
            _depth++;
            if (_depth > _maxDepth)
                throw new InvalidOperationException("Maximum call depth exceeded");
        }

        public void Exit()
        {
            if (_depth > 0)
                _depth--;
        }

        public void Tick()
        {
            _instructions++;
            if (_maxInstructions > 0 && _instructions > _maxInstructions)
                throw new InvalidOperationException("Instruction limit exceeded");
            if (_timeout.HasValue && _stopwatch.Elapsed > _timeout.Value)
                throw new TimeoutException("Execution timed out");
        }

        public void Demand(Capability capability)
        {
            // This limiter does not track capabilities; method provided for interface compatibility.
        }
    }
}
