namespace lizzie.Runtime
{
    /// <summary>
    /// Responsible for loading script modules by name.
    /// </summary>
    public interface IModuleLoader
    {
        /// <summary>
        /// Attempts to load the specified module.
        /// </summary>
        /// <param name="name">Name of the module.</param>
        /// <param name="code">The loaded source code if successful.</param>
        /// <returns><c>true</c> if the module was found; otherwise <c>false</c>.</returns>
        bool TryLoad(string name, out string code);
    }
}
