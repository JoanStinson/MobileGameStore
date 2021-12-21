using JGM.GameStore.Loaders;
using JGM.GameStore.Utils;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace JGM.GameStore.Libraries
{
    public class AssetsLibrary : IAssetsLibrary
    {
        private const string _dataPath = "Data";
        private const string _iconsPath = "UI/ShopItems/Icons";
        private const string _previews3DPath = "UI/ShopItems/Previews3D";
        private const string _fontsPath = "UI/Fonts";

        private AssetLoader<TextAsset> _data;
        private AssetLoader<Sprite> _icons;
        private GameObject[] _previews;
        private Dictionary<string, TMP_FontAsset> _fontAssets;

        public AssetsLibrary()
        {
            _data = new AssetLoader<TextAsset>();
            _data.LoadAllInPath(_dataPath);

            _icons = new AssetLoader<Sprite>();
            _icons.LoadAllInPath(_iconsPath);

            var previews3D = new AssetLoader<GameObject>();
            previews3D.LoadAllInPath(_previews3DPath);
            _previews = previews3D.GetAllAssets().Values.ToArray();

            var fonts = new AssetLoader<Font>();
            fonts.LoadAllInPath(_fontsPath);
            var fontAssetCreator = new FontAssetCreator();
            fontAssetCreator.CreateFromFont(fonts.GetAllAssets().Values.ToArray(), out _fontAssets);
        }

        public TextAsset GetText(in string name) => _data.GetAsset(name);

        public Sprite GetSprite(in string name) => _icons.GetAsset(name);

        public ref readonly GameObject[] Get3DPreviews() => ref _previews;

        public TMP_FontAsset GetFontAsset(in string name)
        {
            if (!_fontAssets.ContainsKey(name))
            {
                return null;
            }

            return _fontAssets[name];
        }
    }
}