using System;

namespace JGM.GameStore.Events
{
    [Serializable]
    public class CustomGameEventGroup
    {
        public GameEvent gameEvent;
        public UnityCustomGameDataEvent onTriggerEvent;
    }
}