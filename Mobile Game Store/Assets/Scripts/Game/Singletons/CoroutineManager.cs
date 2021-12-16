// CoroutineManager.cs
// UI Programmer Test 2021
// 
// Copyright (c) 2021 Ubisoft. All rights reserved.

using System;
using System.Collections;
using UnityEngine;

namespace Ubisoft.UIProgrammerTest
{
	/// <summary>
	/// Auxiliar singleton to trigger coroutines.
	/// </summary>
	public class CoroutineManager : MonoBehaviour
	{
		#region SINGLETON IMPLEMENTATION --------------------------------------
		// Singleton instance
		private static CoroutineManager s_instance = null;
		public static CoroutineManager instance
		{
			get
			{
				if (s_instance == null)
				{
					// Create the object and give it the name of the class
					GameObject singletonObj = new GameObject(typeof(CoroutineManager).Name);

					// Create the instance by adding it as a component of the game object we just created
					// Store its reference so this is only done once
					s_instance = singletonObj.AddComponent<CoroutineManager>();

					// Prevents this game object which has been created by scripts to be saved in the scene if a instance stayed in the scene after playing by mistake
					s_instance.hideFlags = HideFlags.DontSave;
				}
				return s_instance;
			}
		}
		#endregion

		#region PUBLIC METHODS ------------------------------------------------
		/// <summary>
		/// Trigger an action after some delay.
		/// </summary>
		/// <param name="action">Action to be triggered.</param>
		/// <param name="delay">Delay.</param>
		/// <param name="ignoreTimescale">Whether to take timescale in account when delaying the action.</param>
		public static Coroutine DelayedCall(Action action, float delay = 0f, bool ignoreTimescale = true)
		{
			// Launch the coroutine
			return instance.StartCoroutine(instance.DelayedCoroutine(action, delay, ignoreTimescale));
		}

		/// <summary>
		/// Trigger an action after some frames.
		/// </summary>
		/// <param name="action">Action to be triggered.</param>
		/// <param name="delayFrames">Frames to wait before triggereing the action.</param>
		public static Coroutine DelayedCallByFrames(Action action, int delayFrames)
		{
			// Launch the coroutine
			return instance.StartCoroutine(instance.DelayedCoroutineByFrames(action, delayFrames));
		}

		/// <summary>
		/// Static wrapper for the StartCoroutine method, allowing any class to launch 
		/// a coroutine from this manager.
		/// </summary>
		/// <returns>The started coroutine.</returns>
		/// <param name="coroutine">The coroutine to be started.</param>
		public static Coroutine StartExternalCoroutine(IEnumerator coroutine)
		{
			// Just use the instance method
			return instance.StartCoroutine(coroutine);
		}
		#endregion

		#region INTERNAL METHODS ----------------------------------------------
		/// <summary>
		/// Creates a simple coroutine that waits some time before triggering an action.
		/// </summary>
		/// <returns>The coroutine.</returns>
		/// <param name="action">Action to be triggered.</param>
		/// <param name="delay">Delay before triggereing the action.</param>
		/// <param name="ignoreTimescale">Whether to take timescale in account when delaying the action.</param>
		private IEnumerator DelayedCoroutine(Action action, float delay, bool ignoreTimescale)
		{
			// If delay is 0, invoke the action immediately
			if (delay > 0)
			{
				// Wait the target time
				// Ignore time scale?
				if (ignoreTimescale)
				{
					yield return new WaitForSecondsRealtime(delay);
				}
				else
				{
					yield return new WaitForSeconds(delay);
				}
			}

			// Trigger the action
			action.Invoke();
		}

		/// <summary>
		/// Creates a simple coroutine that waits some frames before triggering an action.
		/// </summary>
		/// <returns>The coroutine.</returns>
		/// <param name="action">Action to be triggered.</param>
		/// <param name="delayFrames">Frames to wait before triggereing the action.</param>
		private IEnumerator DelayedCoroutineByFrames(Action action, int delayFrames)
		{
			// If delay is 0, invoke the action immediately
			while (delayFrames > 0)
			{
				delayFrames--;
				yield return new WaitForEndOfFrame();
			}

			// Trigger the action
			action.Invoke();
		}
		#endregion
	}
}
