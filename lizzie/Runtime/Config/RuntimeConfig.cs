namespace lizzie.Runtime.Config
{
    public record RuntimeConfig
    {
        public ExecConfig Exec { get; init; } = new();
        public DeterminismConfig Determinism { get; init; } = new();
    }
}
