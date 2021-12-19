using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Panels
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] private Transform _loadingWindow;
        [SerializeField] private bool _launchError;

        [Inject] 
        private IEventTriggerService _eventTriggerService;

        public async void LoadPurchase(IGameEventData eventData)
        {
            //TODO check transaction and user
            await Task.Delay(TimeSpan.FromSeconds(2));
            if (_launchError)
            {
                _eventTriggerService.Trigger("Purchase Error");
                _loadingWindow.gameObject.SetActive(false);
            }
            else
            {
                //if offer show rewards panel
                var data = eventData as PurchasePackEventData;
                if (data.PackType == Packs.Data.PackData.Type.Offer)
                {
                    _eventTriggerService.Trigger("Purchase Display Offer Rewards", eventData);
                }
                //else add currency to user bitch
                else
                {
                    _eventTriggerService.Trigger("Purchase Success Currency");
                }
                _loadingWindow.gameObject.SetActive(false);
            }
        }
    }
}