using JGM.GameStore.Localization;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.GameStore.Packs.Displayers
{
    public class PackItemDisplayer : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<PackItemDisplayer> { }

        public Image IconImage;
        public LocalizedText AmountText;
    }
}