using JGM.GameStore.Events.Data;
using System.Collections.Generic;
using UnityEngine;

namespace JGM.GameStore.Events.Services
{
    public class GameEventTriggerService : MonoBehaviour, IEventTriggerService
    {
        [SerializeField]
        private GameEvent[] _gameEvents;
        private Dictionary<string, GameEvent> _gameEventsLibrary;

        private void Awake()
        {
            _gameEventsLibrary = new Dictionary<string, GameEvent>();
            for (int i = 0; i < _gameEvents.Length; ++i)
            {
                _gameEventsLibrary.Add(_gameEvents[i].name, _gameEvents[i]);
            }
        }

        public void Trigger(in string eventName, IEventData eventData = null)
        {
            if (!_gameEventsLibrary.ContainsKey(eventName))
            {
                Debug.LogWarning("Trying to trigger an event that doesn't exist!");
                return;
            }
            var gameEvent = _gameEventsLibrary[eventName];
            gameEvent.Trigger(eventData as IGameEventData);
        }
    }
}