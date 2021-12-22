using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Libraries;
using JGM.GameStore.Packs.Displayers;
using System;
using System.Linq;
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
        [SerializeField] private Transform _packItemsParentTransform;
        [Space]
        [SerializeField] private TextMeshProUGUI _dollarsPriceText;
        [SerializeField] private TextMeshProUGUI _gemsPriceText;
        [Space]
        [SerializeField] private Button _dollarsConfirmationButton;
        [SerializeField] private Button _gemsConfirmationButton;
        [Space]
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
            _dollarsConfirmationButton.gameObject.SetActive(false);
            _gemsConfirmationButton.gameObject.SetActive(false);
        }

        public void ShowConfirmationPopup(IGameEventData gameEventData)
        {
            _panelWindow.gameObject.SetActive(true);

            _gameEventData = gameEventData;
            var pack = (gameEventData as PurchasePackEventData).StorePack;
            if (pack.Data.PackCurrency == Transaction.UserProfileService.Currency.Dollars)
            {
                _dollarsPriceText.text = $"{pack.Data.Price}$";
                _dollarsConfirmationButton.gameObject.SetActive(true);
                _gemsConfirmationButton.gameObject.SetActive(false);
            }
            else if (pack.Data.PackCurrency == Transaction.UserProfileService.Currency.Gems)
            {
                _gemsPriceText.text = $"{pack.Data.Price}";
                _gemsConfirmationButton.gameObject.SetActive(true);
                _dollarsConfirmationButton.gameObject.SetActive(false);
            }

            var sortedItems = pack.Data.Items.OrderByDescending(i => i.ItemType).ToArray();
            for (int i = 0; i < sortedItems.Length; ++i)
            {
                var spawnedPackItem = _packItemDisplayerFactory.Create();
                spawnedPackItem.transform.localScale = Vector3.one * _packItemsScale;
                spawnedPackItem.transform.SetParent(_packItemsParentTransform, false);
                if (spawnedPackItem.TryGetComponent<PackItemDisplayer>(out var packItemDisplayer))
                {
                    packItemDisplayer.IconImage.sprite = _assetsLibrary.GetSprite(sortedItems[i].IconName);
                    if (sortedItems[i].ItemType == Packs.Data.PackItemData.Type.Character)
                    {
                        packItemDisplayer.AmountText.RefreshText(sortedItems[i].TextId);
                    }
                    else
                    {
                        packItemDisplayer.AmountText.RefreshText(sortedItems[i].TextId, $"{string.Format("{0:n0}", sortedItems[i].Amount)} ");
                        _particlesTypeToPlay = sortedItems[i].ItemType;
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
            _dollarsConfirmationButton.interactable = false;
            _gemsConfirmationButton.interactable = false;

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

            _dollarsConfirmationButton.interactable = true;
            _gemsConfirmationButton.interactable = true;
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