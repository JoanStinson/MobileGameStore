using JGM.GameStore.Events.Data;
using JGM.GameStore.Events.Services;
using JGM.GameStore.Localization;
using JGM.GameStore.Packs.Data;
using JGM.GameStore.Panels.Helpers;
using JGM.GameStore.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.GameStore.Panels
{
    public class RewardsPanel : MonoBehaviour
    {
        [SerializeField] private Transform _panelWindow;
        [SerializeField] private RawImage _rewardImage;
        [SerializeField] private LocalizedText _amountText;
        [SerializeField] private CurvedText _curvedText;
        [SerializeField] private Camera _prefabCamera;
        [SerializeField] private AudioSource _audioSource;

        [Inject] private PackItem3DVisualizer.Factory _packItem3DVisualizerFactory;
        [Inject] private IEventTriggerService _eventTriggerService;

        private PackItem3DVisualizer _packItem3DVisualizer;
        private Queue<PackItemData> _rewards;
        private string _previousPrefabName = null;

        private void Awake()
        {
            _panelWindow.gameObject.SetActive(false);
            _rewards = new Queue<PackItemData>();
            _packItem3DVisualizer = _packItem3DVisualizerFactory.Create();
            _packItem3DVisualizer.Initialize(_prefabCamera);
        }

        public void ShowRewards(IGameEventData gameEventData)
        {
            var data = (gameEventData as PurchasePackEventData).StorePack.Data;
            var sortedItems = data.Items.OrderBy(i => i.ItemType).ToArray();
            for (int i = 0; i < sortedItems.Length; ++i)
            {
                _rewards.Enqueue(sortedItems[i]);
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
                _eventTriggerService.Trigger("Offer Pack Purchase Success");
                _panelWindow.gameObject.SetActive(false);
                return;
            }

            _audioSource.Play();
            _panelWindow.gameObject.SetActive(true);
            _curvedText.enabled = false;
            await Task.Yield();

            var item = _rewards.Peek();

            if (item.ItemType == PackItemData.Type.Character)
            {
                _amountText.RefreshText(item.TextId, string.Empty);
            }
            else
            {
                _amountText.RefreshText(item.TextId, $"{string.Format("{0:n0}", item.Amount)} ");
            }

            _curvedText.enabled = true;
            _rewardImage.texture = _packItem3DVisualizer.GetRenderTexture(item.PrefabName);
            _previousPrefabName = item.PrefabName;

            _rewards.Dequeue();
        }
    }
}