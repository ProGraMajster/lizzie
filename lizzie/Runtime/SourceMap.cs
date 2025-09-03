using System.Collections.Generic;

namespace lizzie.Runtime
{
    /// <summary>
    /// Describes the mapping between instruction index and original source location.
    /// </summary>
    public record SourceMap(IReadOnlyList<SourceMapEntry> Entries);

    /// <summary>
    /// Represents a single source location entry in a source map.
    /// </summary>
    public record SourceMapEntry(string FileName, int Line, int Column, string Snippet);
}
