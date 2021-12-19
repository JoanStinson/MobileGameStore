using JGM.GameStore.Events.Data;
using System.Collections.Generic;
using UnityEngine;

namespace JGM.GameStore.Events.Services
{
    public class GameEventTriggerService : MonoBehaviour, IEventTriggerService
    {
        [SerializeField]
        private GameEvent[] _gameEventAssets;

        private Dictionary<string, GameEvent> _eventsLibrary;

        private void Awake()
        {
            _eventsLibrary = new Dictionary<string, GameEvent>();
            for (int i = 0; i < _gameEventAssets.Length; ++i)
            {
                _eventsLibrary.Add(_gameEventAssets[i].name, _gameEventAssets[i]);
            }
        }

        public void Trigger(in string eventName, IEventData eventData = null)
        {
            if (!_eventsLibrary.ContainsKey(eventName))
            {
                Debug.LogWarning("Trying to trigger an event that doesn't exist!");
                return;
            }
            var gameEvent = _eventsLibrary[eventName];
            gameEvent.Trigger(eventData as IGameEventData);
        }
    }
}