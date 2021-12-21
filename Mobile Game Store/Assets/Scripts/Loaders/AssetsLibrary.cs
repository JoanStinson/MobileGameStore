using JGM.GameStore.Utils;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace JGM.GameStore.Loaders
{
    public class AssetsLibrary : IAssetsLibrary
    {
        private const string _iconsPath = "UI/ShopItems/Icons";
        private const string _previews3DPath = "UI/ShopItems/Previews3D";
        private const string _fontsPath = "UI/Fonts";

        private AssetLoader<Sprite> _icons;
        private AssetLoader<GameObject> _previews3D;
        private Dictionary<string, TMP_FontAsset> _fontAssets;

        public AssetsLibrary()
        {
            _icons = new AssetLoader<Sprite>();
            _icons.LoadAllInPath(_iconsPath);

            _previews3D = new AssetLoader<GameObject>();
            _previews3D.LoadAllInPath(_previews3DPath);

            var fonts = new AssetLoader<Font>();
            fonts.LoadAllInPath(_fontsPath);
            var fontAssetCreator = new FontAssetCreator();
            fontAssetCreator.CreateFromFont(fonts.GetAllAssets().Values.ToArray(), out _fontAssets);
        }

        public Sprite GetSprite(in string name) => _icons.GetAsset(name);

        public GameObject GetPreview3D(in string name) => _previews3D.GetAsset(name);

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