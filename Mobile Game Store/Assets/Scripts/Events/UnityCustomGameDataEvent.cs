using System;
using UnityEngine.Events;

namespace JGM.GameStore.Events
{
    [Serializable]
    public class UnityCustomGameDataEvent : UnityEvent<IGameEventData> { }
}