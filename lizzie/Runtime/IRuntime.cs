using System.Threading.Tasks;
using lizzie.Runtime.Config;

namespace lizzie.Runtime
{
    public interface IRuntime
    {
        /// <summary>
        /// Compiles the supplied code and returns a compiled module.
        /// </summary>
        CompiledModule Compile(string code, string moduleName = "");

        /// <summary>
        /// Executes a previously compiled module using the supplied configuration.
        /// </summary>
        Task<ScriptValue> RunAsync(CompiledModule module, ExecConfig? execConfig = null);
    }
}
