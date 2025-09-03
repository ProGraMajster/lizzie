using System;

namespace lizzie.exceptions
{
    /// <summary>
    /// Exception thrown when a script fails during compilation or execution.
    /// Carries source information to provide clearer stack traces.
    /// </summary>
    public class ScriptException : Exception
    {
        public string FileName { get; }
        public int Line { get; }
        public int Column { get; }
        public string Snippet { get; }

        public ScriptException(string message, string fileName, int line, int column, string snippet)
            : base(message)
        {
            FileName = fileName;
            Line = line;
            Column = column;
            Snippet = snippet;
        }

        public override string ToString()
        {
            return $"{base.ToString()} at {FileName}:{Line}:{Column}\n{Snippet}";
        }
    }
}
