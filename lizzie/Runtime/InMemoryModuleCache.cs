using System.Collections.Concurrent;

namespace lizzie.Runtime
{
    /// <summary>
    /// Simple in-memory cache for compiled modules keyed by hash and version.
    /// </summary>
    public class InMemoryModuleCache
    {
        private readonly ConcurrentDictionary<string, CompiledModule> _modules = new();

        private static string BuildKey(string hash, string version) => $"{hash}:{version}";

        /// <summary>
        /// Stores the specified module using the supplied hash and version as key.
        /// </summary>
        public void Store(string hash, string version, CompiledModule module)
        {
            _modules[BuildKey(hash, version)] = module;
        }

        /// <summary>
        /// Attempts to retrieve a module using the supplied hash and version.
        /// </summary>
        public bool TryGet(string hash, string version, out CompiledModule module)
        {
            return _modules.TryGetValue(BuildKey(hash, version), out module!);
        }
    }
}
