using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Libraries;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Packs.Displayers
{
    public abstract class PackDisplayer : MonoBehaviour, IPackDisplayer
    {
        public Pack Pack { get; protected set; }

        [Inject] protected IEventTriggerService _eventTriggerService;
        [Inject] protected IAssetsLibrary _assetsLibrary;

        public virtual void PurchasePack()
        {
            var eventData = new PurchasePackEventData(Pack);
            _eventTriggerService.Trigger("Purchase Pack", eventData);
        }

        public virtual void SetPackData(in Pack pack)
        {
            Pack = pack;
        }
    }
}