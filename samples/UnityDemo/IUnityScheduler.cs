using System;

namespace UnityDemo;

public interface IUnityScheduler
{
    void PostToMainThread(Action action);
}
