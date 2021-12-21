using JGM.GameStore.Events.Data;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace JGM.GameStore.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [Serializable]
        public class UnityCustomGameDataEvent : UnityEvent<IGameEventData> { }

        [SerializeField] private GameEvent _gameEvent;
        [SerializeField] private UnityCustomGameDataEvent _onTriggerEvent;

        public void Awake() => _gameEvent?.Register(this);

        public void OnDestroy() => _gameEvent?.Deregister(this);

        public virtual void TriggerEvent(in IGameEventData eventData) => _onTriggerEvent?.Invoke(eventData);
    }
}