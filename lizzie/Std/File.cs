using System;
using System.IO;
using lizzie.Runtime;

namespace lizzie.Std
{
    /// <summary>
    /// File system operations exposed to scripts.
    /// </summary>
    public static class File
    {
        /// <summary>
        /// Reads the contents of a file.
        /// </summary>
        public static string readFile(string path, IResourceLimiter limiter, IFilesystemPolicy policy)
        {
            limiter.Demand(Capability.FileSystem);
            if (!policy.IsPathAllowed(path))
                throw new UnauthorizedAccessException($"Path '{path}' is not allowed.");
            return System.IO.File.ReadAllText(path);
        }

        /// <summary>
        /// Writes text to a file, overwriting any existing content.
        /// </summary>
        public static void writeFile(string path, string contents, IResourceLimiter limiter, IFilesystemPolicy policy)
        {
            limiter.Demand(Capability.FileSystem);
            if (!policy.IsPathAllowed(path))
                throw new UnauthorizedAccessException($"Path '{path}' is not allowed.");
            System.IO.File.WriteAllText(path, contents);
        }

        /// <summary>
        /// Lists directory contents.
        /// </summary>
        public static string[] listDir(string path, IResourceLimiter limiter, IFilesystemPolicy policy)
        {
            limiter.Demand(Capability.FileSystem);
            if (!policy.IsPathAllowed(path))
                throw new UnauthorizedAccessException($"Path '{path}' is not allowed.");
            return Directory.GetFileSystemEntries(path);
        }

        /// <summary>
        /// Creates a directory at the specified path.
        /// </summary>
        public static void createDir(string path, IResourceLimiter limiter, IFilesystemPolicy policy)
        {
            limiter.Demand(Capability.FileSystem);
            if (!policy.IsPathAllowed(path))
                throw new UnauthorizedAccessException($"Path '{path}' is not allowed.");
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Deletes a file or directory.
        /// </summary>
        public static void delete(string path, IResourceLimiter limiter, IFilesystemPolicy policy)
        {
            limiter.Demand(Capability.FileSystem);
            if (!policy.IsPathAllowed(path))
                throw new UnauthorizedAccessException($"Path '{path}' is not allowed.");
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            else if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }
}
