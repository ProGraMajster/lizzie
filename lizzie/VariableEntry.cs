namespace lizzie
{
    /// <summary>
    /// Represents a variable entry with its declared type and current value.
    /// </summary>
    public struct VariableEntry
    {
        public System.Type DeclaredType;
        public object? Value;
        public VariableEntry(System.Type declaredType, object? value)
        {
            DeclaredType = declaredType;
            Value = value;
        }
    }
}
