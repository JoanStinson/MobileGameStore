using UnityEngine;

namespace JGM.GameStore.Utils
{
    public class RotateAxisInLoop : MonoBehaviour
    {
        public enum Axis
        {
            X,
            Y,
            Z
        }

        [SerializeField]
        private Axis _axis = Axis.X;

        [SerializeField]
        [Range(1f, 300f)]
        private float _rotationSpeed = 150f;

        private Vector3 _rotationDirection = Vector3.zero;

        private void Awake()
        {
            switch (_axis)
            {
                case Axis.X:
                    _rotationDirection = Vector3.right;
                    break;

                case Axis.Y:
                    _rotationDirection = Vector3.up;
                    break;

                case Axis.Z:
                    _rotationDirection = Vector3.forward;
                    break;
            }
        }

        private void Update()
        {
            transform.Rotate(_rotationDirection * _rotationSpeed * Time.deltaTime);
        }
    }
}