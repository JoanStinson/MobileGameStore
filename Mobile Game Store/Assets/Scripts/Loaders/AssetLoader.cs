using System.Collections.Generic;
using UnityEngine;

namespace JGM.GameStore.Loaders
{
    //TODO make into a template
    public class AssetLoader : MonoBehaviour, IAssetLoader
    {
        private Sprite[] _sprites;
        private static Dictionary<string, Sprite> _spriteLibrary;

        private void Awake()
        {
            _sprites = Resources.LoadAll<Sprite>("UI/ShopItems/Icons");
            _spriteLibrary = new Dictionary<string, Sprite>();
            foreach (var sprite in _sprites)
            {
                _spriteLibrary.Add(sprite.name, sprite);
            }
        }

        public static Sprite GetSprite(in string name)
        {
            if (_spriteLibrary.ContainsKey(name))
            {
                return _spriteLibrary[name];
            }
            else
            {
                return null;
            }
        }
    }
}