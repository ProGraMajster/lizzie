using System;
using System.Globalization;
using lizzie.exceptions;

namespace lizzie.Std
{
    /// <summary>
    /// Functions for interacting with the host memory store.
    /// </summary>
    public static class Host<TContext>
    {
        /// <summary>
        /// Declares a variable in the host's global memory.
        /// </summary>
        public static Function<TContext> Var => new Function<TContext>((ctx, binder, arguments) =>
        {
            if (arguments.Count == 0)
                throw new LizzieRuntimeException("No arguments provided to 'host-var', provide at least a symbol name, e.g. 'host-var(@foo)'.");

            var symbolName = arguments.Get<string>(0);
            if (symbolName == null)
                throw new LizzieRuntimeException("You'll need to supply a symbol name as a string for 'host-var'.");
            Compiler.SanityCheckSymbolName(symbolName);
            if (binder.Memory.Get(symbolName, out _))
                throw new LizzieRuntimeException($"The symbol '{symbolName}' has already been declared in host memory.");
            var value = arguments.Count > 1 ? arguments.Get(1) : null;
            var entry = new VariableEntry(value?.GetType() ?? typeof(object), value);
            binder.Memory.Create(symbolName, entry);
            return value;
        });

        /// <summary>
        /// Updates a variable stored in the host's global memory.
        /// </summary>
        public static Function<TContext> Set => new Function<TContext>((ctx, binder, arguments) =>
        {
            if (arguments.Count == 0)
                throw new LizzieRuntimeException("No arguments provided to 'host-set', provide at least a symbol name, e.g. 'host-set(@foo)'.");

            var symbolName = arguments.Get<string>(0);
            if (symbolName == null)
                throw new LizzieRuntimeException("You'll need to supply a symbol name as a string for 'host-set'.");
            if (!binder.Memory.Get(symbolName, out var entry))
                throw new LizzieRuntimeException($"The symbol '{symbolName}' has not been declared in host memory.");
            var value = arguments.Count > 1 ? arguments.Get(1) : null;
            var valueType = value?.GetType() ?? typeof(object);
            if (value != null && !entry.DeclaredType.IsAssignableFrom(valueType)) {
                try {
                    var converted = Convert.ChangeType(value, entry.DeclaredType, CultureInfo.InvariantCulture);
                    value = converted;
                } catch {
                    throw new LizzieRuntimeException($"Cannot assign value of type '{valueType.Name}' to '{symbolName}' declared as '{entry.DeclaredType.Name}'.");
                }
            }
            entry.Value = value;
            binder.Memory.Set(symbolName, entry);
            return value;
        });

        /// <summary>
        /// Removes a variable from the host's global memory.
        /// </summary>
        public static Function<TContext> Del => new Function<TContext>((ctx, binder, arguments) =>
        {
            if (arguments.Count == 0)
                throw new LizzieRuntimeException("No arguments provided to 'host-del', provide at least a symbol name, e.g. 'host-del(@foo)'.");
            var symbolName = arguments.Get<string>(0);
            if (symbolName == null)
                throw new LizzieRuntimeException("You'll need to supply a symbol name as a string for 'host-del'.");
            binder.Memory.Remove(symbolName);
            return null;
        });
    }
}
