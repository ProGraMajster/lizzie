using System;

namespace lizzie.Runtime.Config
{
    public record ExecConfig
    {
        /// <summary>
        /// Maximum number of instructions allowed to execute. A value of 0 disables the limit.
        /// </summary>
        public int InstructionLimit { get; init; } = 0;

        /// <summary>
        /// Maximum amount of time the script is allowed to run for. Null disables the timeout.
        /// </summary>
        public TimeSpan? Timeout { get; init; }
    }
}
