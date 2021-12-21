using TMPro;
using UnityEngine;

namespace JGM.GameStore.Loaders
{
    public interface IAssetsLibrary
    {
        Sprite GetSprite(in string name);
        GameObject GetPreview3D(in string name);
        TMP_FontAsset GetFontAsset(in string name);
    }
}