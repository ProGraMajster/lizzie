using System;
using lizzie;

namespace UnityDemo;

public class UnityContext
{
    readonly IUnityScheduler _scheduler;

    public UnityContext(IUnityScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    [Bind(Name = "write")]
    public object Write(Binder<UnityContext> ctx, Arguments args)
    {
        Console.WriteLine(args.Get<string>(0));
        return null;
    }

    [Bind(Name = "main-thread")]
    public object MainThread(Binder<UnityContext> ctx, Arguments args)
    {
        var fn = (Function<UnityContext>)args.Get(0);
        _scheduler.PostToMainThread(() => fn(this, ctx, new Arguments()));
        return null;
    }
}
