using System.Collections.Generic;
using UnityEngine;

namespace JGM.GameStore.Panels.Helpers
{
    public class PackItem3DVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Camera _cameraPrefab;

        private const float _spacingBetweenObjects = 100f;

        private Dictionary<string, GameObject> _renderObjects;

        public void Initialize()
        {
            _renderObjects = new Dictionary<string, GameObject>();
            var previewsParent = new GameObject("3DPreviews").transform;
            var previewPrefabs = Resources.LoadAll<GameObject>("UI/ShopItems/Previews3D");
            for (int i = 0; i < previewPrefabs.Length; ++i)
            {
                var spawnedPreview = Instantiate(previewPrefabs[i]);
                spawnedPreview.transform.SetParent(previewsParent, false);
                spawnedPreview.transform.localPosition += Vector3.right * i * _spacingBetweenObjects;
                var renderTexture = new RenderTexture(256, 256, 0);
                var spawnedCamera = Instantiate(_cameraPrefab);
                spawnedCamera.transform.SetParent(spawnedPreview.transform, false);
                spawnedCamera.GetComponent<Camera>().targetTexture = renderTexture;
                string spawnedPreviewName = spawnedPreview.name.Substring(0, spawnedPreview.name.Length - 7);
                spawnedPreview.name = spawnedPreviewName;
                _renderObjects.Add(spawnedPreviewName, spawnedPreview);
                spawnedPreview.SetActive(false);
            }
        }

        public RenderTexture GetRenderTexture(in string prefabName)
        {
            if (!_renderObjects.ContainsKey(prefabName))
            {
                return null;
            }

            var renderObject = _renderObjects[prefabName];
            renderObject.SetActive(true);
            return renderObject.GetComponentInChildren<Camera>().targetTexture;
        }

        public void DisableGOThatContainsRenderTexture(in string prefabName)
        {
            if (!_renderObjects.ContainsKey(prefabName))
            {
                return;
            }

            _renderObjects[prefabName].SetActive(false);
        }
    }
}