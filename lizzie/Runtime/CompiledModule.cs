namespace lizzie.Runtime
{
    /// <summary>
    /// Represents a compiled script module ready for execution.
    /// </summary>
    public record CompiledModule(string Name, SourceMap SourceMap);
}
