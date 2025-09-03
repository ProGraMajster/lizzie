using System.Collections.Generic;

namespace lizzie.Runtime
{
    /// <summary>
    /// Basic in-memory implementation of <see cref="IBindingRegistry"/>.
    /// </summary>
    public class SimpleBindingRegistry : IBindingRegistry
    {
        private readonly Dictionary<string, object> _bindings = new();

        public void Bind(string name, object value)
        {
            _bindings[name] = value;
        }

        public bool TryGet(string name, out object value)
        {
            return _bindings.TryGetValue(name, out value!);
        }
    }
}
