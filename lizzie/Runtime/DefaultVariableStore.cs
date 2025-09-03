using System.Collections.Concurrent;

namespace lizzie.Runtime
{
    /// <summary>
    /// Default in-memory implementation of <see cref="IVariableStore"/> based on <see cref="ConcurrentDictionary{TKey, TValue}"/>.
    /// </summary>
    public class DefaultVariableStore : IVariableStore
    {
        private readonly ConcurrentDictionary<string, VariableEntry> _store = new();

        public System.Collections.Generic.IEnumerable<string> Keys => _store.Keys;

        public bool Create(string key, VariableEntry entry) => _store.TryAdd(key, entry);

        public bool Get(string key, out VariableEntry entry) => _store.TryGetValue(key, out entry);

        public bool Set(string key, VariableEntry entry)
        {
            _store[key] = entry;
            return true;
        }

        public bool Remove(string key) => _store.TryRemove(key, out _);
    }
}
