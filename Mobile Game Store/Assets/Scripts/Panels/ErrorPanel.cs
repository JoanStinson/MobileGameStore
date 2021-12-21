using JGM.GameStore.Events.Data;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace JGM.GameStore.Panels
{
    public class ErrorPanel : MonoBehaviour
    {
        [SerializeField] 
        private Transform _panelWindow;

        private void Awake()
        {
            _panelWindow.gameObject.SetActive(false);
        }

        public async void ShowErrorMessage(IGameEventData gameEventData)
        {
            _panelWindow.gameObject.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(2));
            _panelWindow.gameObject.SetActive(false);
        }
    }
}