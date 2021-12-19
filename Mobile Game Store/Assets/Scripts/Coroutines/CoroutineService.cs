using System;
using System.Collections;
using UnityEngine;

namespace JGM.GameStore.Coroutines
{
    public sealed class CoroutineService : MonoBehaviour, ICoroutineService
    {
        public Coroutine DelayedCall(Action onCoroutineFinished, float delayInSeconds = 0f, bool ignoreTimescale = true)
        {
            return StartCoroutine(DelayedCoroutine(onCoroutineFinished, delayInSeconds, ignoreTimescale));
        }

        public Coroutine DelayedCallByFrames(Action onCoroutineFinished, int delayInFrames)
        {
            return StartCoroutine(DelayedCoroutineByFrames(onCoroutineFinished, delayInFrames));
        }

        public Coroutine StartExternalCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        private IEnumerator DelayedCoroutine(Action onCoroutineFinished, float delayInSeconds, bool ignoreTimescale)
        {
            if (delayInSeconds > 0)
            {
                if (ignoreTimescale)
                {
                    yield return new WaitForSecondsRealtime(delayInSeconds);
                }
                else
                {
                    yield return new WaitForSeconds(delayInSeconds);
                }
            }

            onCoroutineFinished?.Invoke();
        }

        private IEnumerator DelayedCoroutineByFrames(Action onCoroutineFinished, int delayInFrames)
        {
            while (delayInFrames > 0)
            {
                delayInFrames--;
                yield return new WaitForEndOfFrame();
            }

            onCoroutineFinished?.Invoke();
        }
    }
}