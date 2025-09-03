namespace lizzie.Runtime
{
    /// <summary>
    /// Abstraction over a clock to allow deterministic testing.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Returns the current UTC time.
        /// </summary>
        System.DateTime UtcNow { get; }
    }
}
