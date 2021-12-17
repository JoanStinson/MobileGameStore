using System;
using System.Collections;
using UnityEngine;

namespace Ubisoft.UIProgrammerTest.Singletons
{
    public class CoroutineManager : MonoBehaviour
    {
        public static CoroutineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var singletonObject = new GameObject(typeof(CoroutineManager).Name);
                    _instance = singletonObject.AddComponent<CoroutineManager>();
                    _instance.hideFlags = HideFlags.DontSave;
                }
                return _instance;
            }
        }

        private static CoroutineManager _instance = null;

        public static Coroutine DelayedCall(Action action, float delay = 0f, bool ignoreTimescale = true)
        {
            return Instance.StartCoroutine(Instance.DelayedCoroutine(action, delay, ignoreTimescale));
        }

        public static Coroutine DelayedCallByFrames(Action action, int delayFrames)
        {
            return Instance.StartCoroutine(Instance.DelayedCoroutineByFrames(action, delayFrames));
        }

        public static Coroutine StartExternalCoroutine(IEnumerator coroutine)
        {
            return Instance.StartCoroutine(coroutine);
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