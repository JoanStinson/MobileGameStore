namespace JGM.GameStore.Events
{
    public interface IEventTriggerService
    {
        void Trigger(in string eventName, IEventData eventData = null);
    }
}