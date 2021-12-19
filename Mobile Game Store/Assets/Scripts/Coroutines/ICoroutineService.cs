using System;
using System.Collections;
using UnityEngine;

namespace JGM.GameStore.Coroutines
{
    public interface ICoroutineService
    {
        Coroutine DelayedCall(Action onCoroutineFinished, float delayInSeconds = 0f, bool ignoreTimescale = true);
        Coroutine DelayedCallByFrames(Action onCoroutineFinished, int delayInFrames);
        Coroutine StartExternalCoroutine(IEnumerator coroutine);
    }
}