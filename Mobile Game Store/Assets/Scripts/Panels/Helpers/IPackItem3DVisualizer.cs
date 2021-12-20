using UnityEngine;

namespace JGM.GameStore.Panels.Helpers
{
    public interface IPackItem3DVisualizer
    {
        public void Initialize(Camera cameraPrefab);
        public RenderTexture GetRenderTexture(in string prefabName);
        public void ReturnRenderTexture(in string prefabName);
    }
}