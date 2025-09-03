namespace lizzie.Runtime
{
    /// <summary>
    /// Registry responsible for storing script bindings.
    /// </summary>
    public interface IBindingRegistry
    {
        /// <summary>
        /// Registers a binding with the specified name and value.
        /// </summary>
        void Bind(string name, object value);

        /// <summary>
        /// Attempts to resolve a binding by name.
        /// </summary>
        bool TryGet(string name, out object value);
    }
}
