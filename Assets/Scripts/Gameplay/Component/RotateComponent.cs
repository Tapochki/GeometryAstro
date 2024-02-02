using UnityEngine;

namespace TandC.Gameplay
{
    public interface IRotation
    {
        public void Rotation(Vector2 targetDirection);
    }

    public class OnTargetRotateCompont : IRotation
    {
        private Transform _transform;

        public OnTargetRotateCompont(Transform transform)
        {
            _transform = transform;
        }

        public void Rotation(Vector2 targetDirection)
        {
            Vector2 direction = targetDirection - (Vector2)_transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            _transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }

    public class NoRotationComponent : IRotation
    {
        public void Rotation(Vector2 targetDirection)
        {
        }
    }

    public class InfinitRotate : IRotation
    {
        private Transform _transform;

        public InfinitRotate(Transform transform)
        {
            _transform = transform;
        }

        public void Rotation(Vector2 targetDirection)
        {
            _transform.Rotate(0, 0, 300 * Time.deltaTime);
        }
    }

    public class PlayerRotateComponent : IRotation
    {
        private const float _rotationSpeed = 1000f;
        private Transform _mainTransform;
        private Quaternion _lastRotation;

        public PlayerRotateComponent(Transform transform)
        {
            _mainTransform = transform;
            _lastRotation = _mainTransform.rotation;
        }

        public void Rotation(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                SaveLastRotation();
            }
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            _mainTransform.rotation = Quaternion.RotateTowards(_mainTransform.rotation, toRotation, Time.deltaTime * _rotationSpeed);
            _lastRotation = _mainTransform.rotation;
        }

        public void SaveLastRotation()
        {
            _mainTransform.rotation = _lastRotation;
        }
    }
}