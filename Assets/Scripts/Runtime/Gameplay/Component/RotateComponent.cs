using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IRotation 
    {
        public void SetRotation(Vector3 targetDirection);
        public void Update();
    }

    public class OnTargetRotateComponte : IRotation
    {
        private Transform _transform;

        public OnTargetRotateComponte(Transform transform)
        {
            _transform = transform;

        }
        public void SetRotation(Vector3 targetDirection)
        {
            RotateOnSpawn(targetDirection);
        }

        public void Update()
        {

        }

        private void RotateOnSpawn(Vector3 target)
        {
            Vector2 direction = target - _transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            _transform.eulerAngles = new Vector3(0, 0, angle);
        }

    }

    public class NoRotationComponent : IRotation
    {
        public void SetRotation(Vector3 targetDirection)
        {

        }

        public void Update()
        {

        }
    }

    public class PlayerRotateComponent : IRotation
    {
        private const float _rotationSpeed = 1000f;

        private Transform _mainTransform;
        private Quaternion _lastRotation;
        private Vector2 _direction;

        public PlayerRotateComponent(Transform transform)
        {
            _mainTransform = transform;
            _lastRotation = _mainTransform.rotation;
        }

        public void SetRotation(Vector3 direction)
        {
            _direction = direction;
        }

        public void Update()
        {
            if (_direction == Vector2.zero)
            {
                SaveLastRotation();
            }
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, _direction);
            _mainTransform.rotation = Quaternion.RotateTowards(_mainTransform.rotation, toRotation, Time.deltaTime * _rotationSpeed);
            _lastRotation = _mainTransform.rotation;
        }

        public void SaveLastRotation()
        {
            _mainTransform.rotation = _lastRotation;
        }
    }
}

