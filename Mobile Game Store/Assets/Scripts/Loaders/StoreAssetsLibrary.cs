using UnityEngine;

namespace JGM.GameStore.Loaders
{
    public class StoreAssetsLibrary : IStoreAssetsLibrary
    {
        private AssetLoader<Sprite> _icons;
        private AssetLoader<Font> _fonts;
        private AssetLoader<GameObject> _previews3D;

        private const string _iconsPath = "UI/ShopItems/Icons";
        private const string _fontsPath = "UI/Fonts";
        private const string _previews3DPath = "UI/ShopItems/Previews3D";

        public StoreAssetsLibrary()
        {
            _icons = new AssetLoader<Sprite>();
            _fonts = new AssetLoader<Font>();
            _previews3D = new AssetLoader<GameObject>();
        }

        public void Initialize()
        {
            _icons.LoadAllInPath(_iconsPath);
            //_fonts.LoadAllInPath(_fontsPath);
            //_previews3D.LoadAllInPath(_previews3DPath);
        }

        public Sprite GetSprite(in string name) => _icons.GetAsset(name);

        public Font GetFont(in string name) => _fonts.GetAsset(name);

        public GameObject GetPreview3D(in string name) => _previews3D.GetAsset(name);
    }
}