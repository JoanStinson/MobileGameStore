using JGM.GameStore.Events.Data;
using System;
using UnityEngine.Events;

namespace JGM.GameStore.Events.Custom
{
    [Serializable]
    public class UnityCustomGameDataEvent : UnityEvent<IGameEventData> { }
}