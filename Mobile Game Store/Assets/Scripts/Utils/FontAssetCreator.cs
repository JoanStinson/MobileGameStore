using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JGM.GameStore.Utils
{
    public class FontAssetCreator
    {
        public void CreateFromFont(in Font font, out TMP_FontAsset fontAsset)
        {
            fontAsset = TMP_FontAsset.CreateFontAsset(font);
            fontAsset.name = $"{font.name} SDF";
        }

        public void CreateFromFont(in Font[] fonts, out Dictionary<string, TMP_FontAsset> fontAssets)
        {
            fontAssets = new Dictionary<string, TMP_FontAsset>();
            for (int i = 0; i < fonts.Length; ++i)
            {
                CreateFromFont(fonts[i], out var fontAsset);
                fontAssets.Add(fonts[i].name, fontAsset);
            }
        }
    }
}