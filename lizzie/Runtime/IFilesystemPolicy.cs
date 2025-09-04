namespace lizzie.Runtime
{
    /// <summary>
    /// Defines a contract for sandbox policies that restrict filesystem access.
    /// </summary>
    public interface IFilesystemPolicy
    {
        /// <summary>
        /// Determines if a path is permitted by the policy.
        /// </summary>
        /// <param name="path">Absolute or relative file system path.</param>
        /// <returns><c>true</c> if the path is allowed; otherwise, <c>false</c>.</returns>
        bool IsPathAllowed(string path);
    }
}
