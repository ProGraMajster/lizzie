using System;

namespace UnityDemo;

public class MockUnityScheduler : IUnityScheduler
{
    public void PostToMainThread(Action action)
    {
        Console.WriteLine("Running action on mock main thread...");
        action();
    }
}
