using System;
using lizzie;
using lizzie.Runtime;
using UnityDemo;

var ctx = RuntimeProfiles.UnityDefaults();
var scheduler = new MockUnityScheduler();
var binder = new Binder<IScriptContext>();
LambdaCompiler.BindFunctions(binder);

binder["write"] = new Function<IScriptContext>((c, b, args) =>
{
    Console.WriteLine(args.Get<string>(0));
    return null;
});

binder["main-thread"] = new Function<IScriptContext>((c, b, args) =>
{
    var fn = (Function<IScriptContext>)args.Get(0);
    scheduler.PostToMainThread(() => fn(c, b, new Arguments()));
    return null;
});

var lambda = LambdaCompiler.Compile(ctx, binder, "main-thread(function() { write('Hello from Unity demo') })");
lambda();

