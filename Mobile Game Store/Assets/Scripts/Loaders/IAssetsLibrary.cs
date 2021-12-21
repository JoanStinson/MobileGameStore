using TMPro;
using UnityEngine;

namespace JGM.GameStore.Loaders
{
    public interface IAssetsLibrary
    {
        TextAsset GetText(in string name);
        Sprite GetSprite(in string name);
        ref readonly GameObject[] Get3DPreviews();
        TMP_FontAsset GetFontAsset(in string name);
    }
}