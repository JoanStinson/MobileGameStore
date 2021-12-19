using JGM.GameStore.Events.Data;

namespace JGM.GameStore.Events.Services
{
    public interface IEventTriggerService
    {
        void Trigger(in string eventName, IEventData eventData = null);
    }
}