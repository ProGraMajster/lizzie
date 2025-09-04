using System;
using FileModule = lizzie.Std.File;

namespace lizzie.Runtime
{
    /// <summary>
    /// Helpers for registering standard library functions in a binding registry.
    /// </summary>
    public static class StdBindingExtensions
    {
        /// <summary>
        /// Registers file system functions in the provided script context's binding registry.
        /// </summary>
        public static void BindFileModule(this IScriptContext ctx)
        {
            if (ctx.Sandbox is not IFilesystemPolicy fs)
                return;
            var limiter = ctx.Resources;
            ctx.Bindings.Bind("readFile", (Func<string, string>)(path => FileModule.readFile(path, limiter, fs)));
            ctx.Bindings.Bind("writeFile", (Action<string, string>)((path, contents) => FileModule.writeFile(path, contents, limiter, fs)));
            ctx.Bindings.Bind("listDir", (Func<string, string[]>)(path => FileModule.listDir(path, limiter, fs)));
            ctx.Bindings.Bind("createDir", (Action<string>)(path => FileModule.createDir(path, limiter, fs)));
            ctx.Bindings.Bind("delete", (Action<string>)(path => FileModule.delete(path, limiter, fs)));
        }
    }
}
