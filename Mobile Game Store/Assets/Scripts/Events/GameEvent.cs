using JGM.GameStore.Events.Data;
using System.Collections.Generic;
using UnityEngine;

namespace JGM.GameStore.Events
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        private HashSet<GameEventListener> _listeners = new HashSet<GameEventListener>();

        public void Register(GameEventListener listener) => _listeners.Add(listener);

        public void Deregister(GameEventListener listener) => _listeners.Remove(listener);

        public void Trigger(in IGameEventData eventData = null)
        {
            Debug.Log($"'<color=cyan>{name}</color>' game event was triggered!");
            foreach (var listener in _listeners)
            {
                listener?.TriggerEvent(eventData);
            }
        }
    }
}