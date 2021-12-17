using System;
using System.Collections;
using UnityEngine;

namespace JGM.GameStore.Coroutines
{
    public sealed class CoroutineService : MonoBehaviour, ICoroutineService
    {
        public Coroutine DelayedCall(Action action, float delay = 0f, bool ignoreTimescale = true)
        {
            return StartCoroutine(DelayedCoroutine(action, delay, ignoreTimescale));
        }

        public Coroutine DelayedCallByFrames(Action action, int delayFrames)
        {
            return StartCoroutine(DelayedCoroutineByFrames(action, delayFrames));
        }

        public Coroutine StartExternalCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        private IEnumerator DelayedCoroutine(Action action, float delay, bool ignoreTimescale)
        {
            if (delay > 0)
            {
                if (ignoreTimescale)
                {
                    yield return new WaitForSecondsRealtime(delay);
                }
                else
                {
                    yield return new WaitForSeconds(delay);
                }
            }

            action?.Invoke();
        }

        private IEnumerator DelayedCoroutineByFrames(Action action, int delayFrames)
        {
            while (delayFrames > 0)
            {
                delayFrames--;
                yield return new WaitForEndOfFrame();
            }

            action?.Invoke();
        }
    }
}