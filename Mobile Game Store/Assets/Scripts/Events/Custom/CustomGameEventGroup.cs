using System;

namespace JGM.GameStore.Events.Custom
{
    [Serializable]
    public class CustomGameEventGroup
    {
        public GameEvent gameEvent;
        public UnityCustomGameDataEvent onTriggerEvent;
    }
}