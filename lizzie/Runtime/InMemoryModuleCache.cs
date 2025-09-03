using System.Collections.Concurrent;

namespace lizzie.Runtime
{
    /// <summary>
    /// Simple in-memory cache for compiled modules.
    /// </summary>
    public class InMemoryModuleCache
    {
        private readonly ConcurrentDictionary<string, CompiledModule> _modules = new();

        public void Store(CompiledModule module)
        {
            _modules[module.Name] = module;
        }

        public bool TryGet(string name, out CompiledModule module)
        {
            return _modules.TryGetValue(name, out module!);
        }
    }
}
