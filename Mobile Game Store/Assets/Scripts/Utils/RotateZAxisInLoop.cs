using UnityEngine;

namespace JGM.GameStore.Utils
{
    public class RotateZAxisInLoop : MonoBehaviour
    {
        [SerializeField]
        [Range(1f, 300f)]
        private float _rotationSpeed = 150f;

        private void Update()
        {
            transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
        }
    }
}