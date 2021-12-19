using JGM.GameStore.Events;
using JGM.GameStore.Loaders;
using JGM.GameStore.Packs.Displayers;
using TMPro;
using UnityEngine;
using Zenject;

namespace JGM.GameStore.Panels
{
    public class ConfirmationPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private Transform _itemsParent;
        [SerializeField] private GameObject _itemPrefab;
        [Inject]
        private IStoreAssetsLibrary _assetsLibrary;
        [Inject]
        private IEventTriggerService _eventTriggerService;
        private IGameEventData _eventData;

        public void SetPanelData(IGameEventData gameEventData)
        {
            var data = gameEventData as PurchasePackEventData;
            _price.text = data.Price.ToString();
            for (int i = 0; i < data.Items.Length; ++i)
            {
                var go = Instantiate(_itemPrefab);
                go.transform.SetParent(_itemsParent, false);
                var displayer = go.GetComponent<StoreItemDisplayerData>();
                displayer.Icon.sprite = _assetsLibrary.GetSprite(data.Items[i].IconName);
                displayer.Amount.text = data.Items[i].Amount.ToString();
            }
            _eventData = gameEventData;
        }

        public void DisableAndSendLoadingPurchaseEvent()
        {
            _eventTriggerService.Trigger("Loading Purchase", _eventData);
            DestroyItems();
        }

        public void CancelPurchase()
        {
            _eventTriggerService.Trigger("Cancel Purchase");
            DestroyItems();
        }


        private void DestroyItems()
        {
            for (int i = 0; i < _itemsParent.childCount; ++i)
            {
                Destroy(_itemsParent.GetChild(i).gameObject);
            }
        }
    }
}