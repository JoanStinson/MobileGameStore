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
        [Inject] private IEventTriggerService _trigg;

        public async void LoadPurchase(IGameEventData eventData)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            if (_launchError)
            {
                _trigg.Trigger("Purchase Error");
                _loadingWindow.gameObject.SetActive(false);
            }
            else
            {
                //if offer show rewards panel

                //else add currency to user bitch
            }
        }
    }
}