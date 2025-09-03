using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using lizzie.Runtime.Config;
using lizzie.exceptions;

namespace lizzie.Runtime
{
    /// <summary>
    /// Default runtime responsible for compiling and executing script modules.
    /// </summary>
    public class DefaultRuntime : IRuntime
    {
        private readonly InMemoryModuleCache _cache = new();
        private readonly RuntimeConfig _config;
        private readonly Random _rng;

        public DefaultRuntime(RuntimeConfig? config = null)
        {
            _config = config ?? new RuntimeConfig();
            var seed = _config.Determinism.RandomSeed;
            _rng = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        /// <summary>
        /// Compiles the supplied code into a module, generates source map data and stores it in the cache.
        /// </summary>
        public CompiledModule Compile(string code, string moduleName = "")
        {
            var lines = code.Replace("\r\n", "\n").Split('\n');
            var entries = new List<SourceMapEntry>(lines.Length);
            for (var i = 0; i < lines.Length; i++)
            {
                entries.Add(new SourceMapEntry(moduleName, i + 1, 1, lines[i]));
            }
            var module = new CompiledModule(moduleName, new SourceMap(entries));
            _cache.Store(module);
            return module;
        }

        /// <summary>
        /// Executes a previously compiled module using the supplied execution configuration.
        /// Performs instruction counting and timeout checks.
        /// </summary>
        public async Task<ScriptValue> RunAsync(CompiledModule module, ExecConfig? execConfig = null)
        {
            execConfig ??= new ExecConfig();
            var limit = execConfig.InstructionLimit;
            var timeout = execConfig.Timeout;
            var sw = Stopwatch.StartNew();
            int executed = 0;
            foreach (var entry in module.SourceMap.Entries)
            {
                executed++;
                if (limit > 0 && executed > limit)
                {
                    throw new ScriptException("Instruction limit exceeded", entry.FileName, entry.Line, entry.Column, entry.Snippet);
                }
                if (timeout.HasValue && sw.Elapsed > timeout.Value)
                {
                    throw new ScriptException("Execution timed out", entry.FileName, entry.Line, entry.Column, entry.Snippet);
                }
                // Yield to simulate execution and provide deterministic RNG usage
                _ = _rng.Next();
                await Task.Yield();
            }
            return ScriptValue.Null;
        }
    }
}
