using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Packs.Displayers.Utils;
using JGM.GameStore.Panels.Helpers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.GameStore.Panels
{
    public class RewardsPanel : MonoBehaviour
    {
        [SerializeField] private RawImage _rewardImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private Transform _rewardsPopupTransform;
        [SerializeField] private Camera _prefabCamera;

        [Inject]
        private IEventTriggerService _eventTriggerService;
        private IPackItem3DVisualizer _packItem3DVisualizer;
        private Queue<PackItemData> _rewards;
        private string _previousPrefabName = null;

        private void Awake()
        {
            _rewards = new Queue<PackItemData>();
            _packItem3DVisualizer = new PackItem3DVisualizer();
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

        public void ShowNextReward()
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

            var item = _rewards.Peek();

            if (item.ItemType == PackItemData.Type.Character)
            {
                var nameConverter = new CharacterNameConverter();
                nameConverter.GetCharacterNameFromId(item.ItemId, out var characterName);
                _amountText.text = characterName;
            }
            else if (item.ItemType == PackItemData.Type.Gems)
            {
                _amountText.text = $"{item.Amount} Gems";
            }
            else if (item.ItemType == PackItemData.Type.Coins)
            {
                _amountText.text = $"{item.Amount} Coins";
            }

            _rewardImage.texture = _packItem3DVisualizer.GetRenderTexture(item.PrefabName);
            _previousPrefabName = item.PrefabName;

            _rewards.Dequeue();
        }
    }
}