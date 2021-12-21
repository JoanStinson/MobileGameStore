using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Libraries;
using JGM.GameStore.Packs.Displayers;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.GameStore.Panels
{
    public class ConfirmationPanel : MonoBehaviour
    {
        [SerializeField] private Transform _panelWindow;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _packItemsParentTransform;
        [SerializeField] private Button _confirmPurchaseButton;
        [SerializeField] private ParticleSystem _coinsBurstFx;
        [SerializeField] private ParticleSystem _gemsBurstFx;

        [Inject] private IAssetsLibrary _assetsLibrary;
        [Inject] private IEventTriggerService _eventTriggerService;
        [Inject] private PackItemDisplayer.Factory _packItemDisplayerFactory;

        private const float _packItemsScale = 1.2f;

        private IGameEventData _gameEventData;
        private Packs.Data.PackItemData.Type _particlesTypeToPlay;
        private bool _shouldPlayCoinsParticles;
        private bool _shouldPlayGemsParticles;

        private void Awake()
        {
            _panelWindow.gameObject.SetActive(false);
        }

        public void ShowConfirmationPopup(IGameEventData gameEventData)
        {
            _panelWindow.gameObject.SetActive(true);
            _confirmPurchaseButton.interactable = true;

            _gameEventData = gameEventData;
            var data = (gameEventData as PurchasePackEventData).StorePack.Data;
            _priceText.text = (data.PackCurrency == Transaction.UserProfileService.Currency.Gems) ? $"{data.Price}" : $"{data.Price}$";

            for (int i = 0; i < data.Items.Length; ++i)
            {
                var spawnedPackItem = _packItemDisplayerFactory.Create();
                spawnedPackItem.transform.localScale = Vector3.one * _packItemsScale;
                spawnedPackItem.transform.SetParent(_packItemsParentTransform, false);
                if (spawnedPackItem.TryGetComponent<PackItemDisplayer>(out var packItemDisplayer))
                {
                    packItemDisplayer.IconImage.sprite = _assetsLibrary.GetSprite(data.Items[i].IconName);
                    if (data.Items[i].ItemType == Packs.Data.PackItemData.Type.Character)
                    {
                        packItemDisplayer.AmountText.RefreshText(data.Items[i].TextId);
                    }
                    else
                    {
                        packItemDisplayer.AmountText.RefreshText(data.Items[i].TextId, $"{string.Format("{0:n0}", data.Items[i].Amount)} ");
                        _particlesTypeToPlay = data.Items[i].ItemType;
                    }
                }
            }
        }

        public void ShowConfirmationPopupAnimation(IGameEventData gameEventData)
        {
            ShowConfirmationPopup(gameEventData);
            PlayAnimation();
        }

        public void ConfirmPurchase()
        {
            _eventTriggerService.Trigger("Processing Purchase", _gameEventData);
            DestroyPackItems();
        }

        public void CancelPurchase()
        {
            _eventTriggerService.Trigger("Cancel Purchase");
            DestroyPackItems();
        }

        private void Update()
        {
            if (_shouldPlayCoinsParticles && !_coinsBurstFx.isPlaying)
            {
                _coinsBurstFx.Play();
            }
            else if (_shouldPlayGemsParticles && !_gemsBurstFx.isPlaying)
            {
                _gemsBurstFx.Play();
            }
        }

        private async void PlayAnimation()
        {
            _confirmPurchaseButton.interactable = false;

            if (_particlesTypeToPlay == Packs.Data.PackItemData.Type.Coins)
            {
                _shouldPlayCoinsParticles = true;
                await Task.Delay(TimeSpan.FromSeconds(_coinsBurstFx.main.duration));
                _shouldPlayCoinsParticles = false;
                _coinsBurstFx.Stop();
            }
            else if (_particlesTypeToPlay == Packs.Data.PackItemData.Type.Gems)
            {
                _shouldPlayGemsParticles = true;
                await Task.Delay(TimeSpan.FromSeconds(_gemsBurstFx.main.duration));
                _shouldPlayGemsParticles = false;
                _gemsBurstFx.Stop();
            }

            _eventTriggerService.Trigger("Currency Pack Purchase Success");
            _panelWindow.gameObject.SetActive(false);
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