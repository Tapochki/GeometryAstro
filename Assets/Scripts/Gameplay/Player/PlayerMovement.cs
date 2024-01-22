using ChebDoorStudio.ScriptableObjects;
using UnityEngine;

namespace ChebDoorStudio.Gameplay.Player
{
    public sealed class PlayerMovement
    {
        private Transform _selfTransform;

        private InitialGameData _initialGameData;

        private float _rotatingSpeed;

        private Vector3 _rotation;

        public PlayerMovement(Transform selfTransform, InitialGameData initialGameData)
        {
            _selfTransform = selfTransform;
            _initialGameData = initialGameData;

            _rotatingSpeed = _initialGameData.playerData.rotatingSpeed;
            ResetRotation();

            _rotation = new Vector3(0, 0, _rotatingSpeed);
        }

        public void ResetRotation()
        {
            _selfTransform.localEulerAngles = _initialGameData.playerData.initialRotate;
        }

        public void UpdateMovementDirection()
        {
            _rotation *= -1.0f;
        }

        public void FixedUpdate()
        {
            _selfTransform.Rotate(_rotation * Time.fixedDeltaTime);
        }
    }
}