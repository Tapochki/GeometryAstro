using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Smoothly rotates a Transform toward an input direction at 1000 deg/sec.
    /// Used by the player ship body.
    /// </summary>
    public class PlayerRotateComponent : IRotation
    {
        private const float RotationSpeed = 1000f;

        private readonly Transform _transform;
        private Quaternion _lastRotation;
        private Vector2 _direction;

        public PlayerRotateComponent(Transform transform)
        {
            _transform = transform;
            _lastRotation = _transform.rotation;
        }

        public void SetRotation(Vector3 targetDirection)
        {
            _direction = targetDirection;
        }

        public void Update()
        {
            if (_direction == Vector2.zero)
            {
                _transform.rotation = _lastRotation;
                return;
            }

            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, _direction);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, toRotation, Time.deltaTime * RotationSpeed);
            _lastRotation = _transform.rotation;
        }
    }
}
