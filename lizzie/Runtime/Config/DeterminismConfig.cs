namespace lizzie.Runtime.Config
{
    public record DeterminismConfig
    {
        /// <summary>
        /// Optional seed used to create deterministic random number generation.
        /// </summary>
        public int? RandomSeed { get; init; }
    }
}
