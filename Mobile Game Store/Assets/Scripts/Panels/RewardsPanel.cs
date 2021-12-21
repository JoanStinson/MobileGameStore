using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Localization;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Panels.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.GameStore.Panels
{
    public class RewardsPanel : MonoBehaviour
    {
        [SerializeField] private RawImage _rewardImage;
        [SerializeField] private LocalizedText _amountText;
        [SerializeField] private Transform _rewardsPopupTransform;
        [SerializeField] private Camera _prefabCamera;

        [Inject] private PackItem3DVisualizer.Factory _packItem3DVisualizerFactory;
        [Inject] private IEventTriggerService _eventTriggerService;

        private PackItem3DVisualizer _packItem3DVisualizer;
        private Queue<PackItemData> _rewards;
        private string _previousPrefabName = null;

        private void Awake()
        {
            _rewards = new Queue<PackItemData>();
            _packItem3DVisualizer = _packItem3DVisualizerFactory.Create();
            _packItem3DVisualizer.Initialize(_prefabCamera);
        }

        public void ShowRewards(IGameEventData gameEventData)
        {
            var data = (gameEventData as PurchasePackEventData).StorePack.Data;
            for (int i = data.Items.Length - 1; i >= 0; --i)
            {
                _rewards.Enqueue(data.Items[i]);
            }
            ShowNextReward();
        }

        public async void ShowNextReward()
        {
            if (_previousPrefabName != null)
            {
                _packItem3DVisualizer.ReturnRenderTexture(_previousPrefabName);
            }

            if (_rewards.Count < 1)
            {
                _eventTriggerService.Trigger("Purchase Success Rewards");
                _rewardsPopupTransform.gameObject.SetActive(false);
                return;
            }

            _rewardsPopupTransform.gameObject.SetActive(true);
            await Task.Yield();

            var item = _rewards.Peek();

            if (item.ItemType == PackItemData.Type.Character)
            {
                _amountText.RefreshText(item.TextId, string.Empty);
            }
            else 
            {
                _amountText.RefreshText(item.TextId, $"{item.Amount} ");
            }

            _rewardImage.texture = _packItem3DVisualizer.GetRenderTexture(item.PrefabName);
            _previousPrefabName = item.PrefabName;

            _rewards.Dequeue();
        }
    }
}