namespace lizzie.Runtime
{
    /// <summary>
    /// Represents a storage abstraction for global variables.
    /// </summary>
    public interface IVariableStore
    {
        /// <summary>
        /// Creates a new variable entry. Returns false if the key already exists.
        /// </summary>
        bool Create(string key, VariableEntry entry);

        /// <summary>
        /// Gets the variable entry associated with the specified key.
        /// </summary>
        bool Get(string key, out VariableEntry entry);

        /// <summary>
        /// Sets the variable entry for the specified key. Returns false if the key does not exist.
        /// </summary>
        bool Set(string key, VariableEntry entry);

        /// <summary>
        /// Removes the specified key from the store.
        /// </summary>
        bool Remove(string key);

        /// <summary>
        /// Enumerates all keys currently stored.
        /// </summary>
        System.Collections.Generic.IEnumerable<string> Keys { get; }
    }
}
