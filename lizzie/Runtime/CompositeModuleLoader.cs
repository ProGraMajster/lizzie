using System.Collections.Generic;
using System.IO;

namespace lizzie.Runtime
{
    /// <summary>
    /// Module loader that checks embedded modules before falling back to disk.
    /// </summary>
    public class CompositeModuleLoader : IModuleLoader
    {
        private readonly IDictionary<string, string> _embedded;
        private readonly string _basePath;

        public CompositeModuleLoader(IDictionary<string, string>? embedded = null, string? basePath = null)
        {
            _embedded = embedded ?? new Dictionary<string, string>();
            _basePath = basePath ?? Directory.GetCurrentDirectory();
        }

        public bool TryLoad(string name, out string code)
        {
            if (_embedded.TryGetValue(name, out code))
            {
                return true;
            }

            var path = Path.Combine(_basePath, name);
            if (File.Exists(path))
            {
                code = File.ReadAllText(path);
                return true;
            }

            code = string.Empty;
            return false;
        }
    }
}
