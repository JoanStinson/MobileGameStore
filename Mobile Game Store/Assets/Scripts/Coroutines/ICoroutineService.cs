using System;
using System.Collections;
using UnityEngine;

namespace JGM.GameStore.Coroutines
{
    public interface ICoroutineService
    {
        Coroutine DelayedCall(Action action, float delay = 0f, bool ignoreTimescale = true);
        Coroutine DelayedCallByFrames(Action action, int delayFrames);
        Coroutine StartExternalCoroutine(IEnumerator coroutine);
    }
}