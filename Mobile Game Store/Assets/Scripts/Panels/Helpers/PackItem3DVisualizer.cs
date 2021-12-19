using System.Collections.Generic;
using UnityEngine;

namespace JGM.GameStore.Rewards
{
    public class PackItem3DVisualizer : MonoBehaviour
    {
        [SerializeField] private Camera _cameraPrefab;

        private const float _spacingBetweenObjects = 100f;

        private Dictionary<string, GameObject> _renderObjects;
        private int _lastIndex = 0;
        private Transform _previewsParent;

        private void Awake()
        {
            _renderObjects = new Dictionary<string, GameObject>();
            _previewsParent = new GameObject("3DPreviews").transform;
        }

        public void Store3DPreview(in string prefabName)
        {
            if (_renderObjects.ContainsKey(prefabName))
            {
                return;
            }

            var prefab = Resources.Load<GameObject>($"UI/ShopItems/Previews3D/{prefabName}");
            var objectPreview = Instantiate(prefab);
            objectPreview.transform.SetParent(_previewsParent, false);
            objectPreview.transform.localPosition += Vector3.right * _lastIndex * _spacingBetweenObjects;
            var renderTexture = new RenderTexture(256, 256, 0);
            var camera = Instantiate(_cameraPrefab);
            camera.transform.SetParent(objectPreview.transform, false);
            camera.GetComponent<Camera>().targetTexture = renderTexture;
            _renderObjects.Add(prefabName, objectPreview);
            objectPreview.SetActive(false);
            _lastIndex++;
        }

        public RenderTexture GetRenderTexture(in string prefabName)
        {
            if (!_renderObjects.ContainsKey(prefabName))
            {
                return null;
            }

            var a = _renderObjects[prefabName];
            return a.GetComponentInChildren<Camera>().targetTexture;
        }
    }
}