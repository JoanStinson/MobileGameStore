using UnityEngine;

namespace JGM.GameStore.Loaders
{
    public interface IAssetsLibrary
    {
        Sprite GetSprite(in string name);
        Font GetFont(in string name);
        GameObject GetPreview3D(in string name);
    }
}