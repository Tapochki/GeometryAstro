using UnityEngine;

namespace TandC.Gameplay
{
    public class GameplayInputHandler : MonoBehaviour, IGameplayInputHandler
    {
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _rotationJoystick;

        public Vector2 MoveDirection { get; private set; }
        public Vector2 RotationDirection { get; private set; }

        private void FixedUpdate()
        {
#if UNITY_EDITOR
            SimulateInputInEditor();
#else
            UpdateInputFromJoysticks();
#endif
        }

        private void SimulateInputInEditor()
        {
            MoveDirection = _moveJoystick.Direction == Vector2.zero
                ? new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))
                : _moveJoystick.Direction;

            RotationDirection = _rotationJoystick.Direction;
        }

        private void UpdateInputFromJoysticks()
        {
            MoveDirection = _moveJoystick.Direction;
            RotationDirection = _rotationJoystick.Direction;
        }
    }
}