using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Loaders;
using JGM.GameStore.Packs.Displayers;
using JGM.GameStore.Packs.Displayers.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.GameStore.Panels
{
    public class ConfirmationPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _popup;
        [SerializeField] private Transform _packItemsParentTransform;
        [SerializeField] private Button _confirmPurchaseButton;
        [SerializeField] private GameObject _packItemPrefab;
        [SerializeField] private ParticleSystem _particleSystem;

        [Inject] private IAssetsLibrary _assetsLibrary;
        [Inject] private IEventTriggerService _eventTriggerService;

        private IGameEventData _gameEventData;
        private bool _shouldPlayParticles;

        public void ShowConfirmationPopup(IGameEventData gameEventData)
        {
            _gameEventData = gameEventData;
            _confirmPurchaseButton.interactable = true;
            var data = (gameEventData as PurchasePackEventData).StorePack.Data;
            _priceText.text = data.Price.ToString();

            for (int i = 0; i < data.Items.Length; ++i)
            {
                var spawnedPackItem = Instantiate(_packItemPrefab);
                spawnedPackItem.transform.SetParent(_packItemsParentTransform, false);
                if (spawnedPackItem.TryGetComponent<PackItemDisplayer>(out var packItemDisplayer))
                {
                    packItemDisplayer.IconImage.sprite = _assetsLibrary.GetSprite(data.Items[i].IconName);
                    if (data.Items[i].ItemType == Packs.Data.PackItemData.Type.Character)
                    {
                        var nameConverter = new CharacterNameConverter();
                        nameConverter.GetCharacterNameFromId(data.Items[i].ItemId, out var characterName);
                        packItemDisplayer.AmountText.text = characterName;
                    }
                    else if (data.Items[i].ItemType == Packs.Data.PackItemData.Type.Gems)
                    {
                        packItemDisplayer.AmountText.text = $"{data.Items[i].Amount} Coins";
                    }
                    else if (data.Items[i].ItemType == Packs.Data.PackItemData.Type.Coins)
                    {
                        packItemDisplayer.AmountText.text = $"{data.Items[i].Amount} Gems";
                    }
                }
            }
        }

        public void ShowConfirmationPopupAnimation(IGameEventData gameEventData)
        {
            ShowConfirmationPopup(gameEventData);
            StartCoroutine(PlayAnimation());
        }

        public void ConfirmPurchase()
        {
            _eventTriggerService.Trigger("Loading Purchase", _gameEventData);
            DestroyPackItems();
        }

        public void CancelPurchase()
        {
            _eventTriggerService.Trigger("Cancel Purchase");
            DestroyPackItems();
        }

        private void Update()
        {
            if (_shouldPlayParticles && !_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }
        }

        private IEnumerator PlayAnimation()
        {
            _confirmPurchaseButton.interactable = false;
            _shouldPlayParticles = true;
            yield return new WaitForSeconds(_particleSystem.main.duration);
            _shouldPlayParticles = false;
            _particleSystem.Stop();
            _eventTriggerService.Trigger("Purchase Success Currency");
            _popup.gameObject.SetActive(false);
            DestroyPackItems();
        }

        private void DestroyPackItems()
        {
            for (int i = 0; i < _packItemsParentTransform.childCount; ++i)
            {
                Destroy(_packItemsParentTransform.GetChild(i).gameObject);
            }
        }
    }
}