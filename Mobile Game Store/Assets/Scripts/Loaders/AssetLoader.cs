using System.Collections.Generic;
using UnityEngine;

namespace JGM.GameStore.Loaders
{
    public class AssetLoader<T> where T : Object
    {
        private Dictionary<string, T> _assetsLibrary;

        public void LoadAllInPath(in string resourcesPath)
        {
            T[] _assets = Resources.LoadAll<T>(resourcesPath);
            _assetsLibrary = new Dictionary<string, T>();
            for (int i = 0; i < _assets.Length; ++i)
            {
                _assetsLibrary.Add(_assets[i].name, _assets[i]);
            }
        }

        public T GetAsset(in string assetName)
        {
            if (_assetsLibrary.ContainsKey(assetName))
            {
                return _assetsLibrary[assetName];
            }
            else
            {
                Debug.LogWarning($"{assetName} was not found!");
                return null;
            }
        }

        public ref readonly Dictionary<string, T> GetAllAssets()
        {
            return ref _assetsLibrary;
        }
    }
}