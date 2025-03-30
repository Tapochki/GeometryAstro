using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class GameplayInputHandler : MonoBehaviour, IGameplayInputHandler
    {
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _rotationJoystick;

        [SerializeField] private RocketInputButton _rocketButton;
        [SerializeField] private SkillInputButton _cloakButton;
        [SerializeField] private SkillInputButton _dashButton;

        public Vector2 MoveDirection { get; private set; }
        public Vector2 RotationDirection { get; private set; }

        public RocketInputButton RocketButton => _rocketButton;
        public SkillInputButton CloakButton => _cloakButton;
        public SkillInputButton DashButton => _dashButton;

        public void Init() 
        {
            _rocketButton.DeActivate();
            _cloakButton.DeActivate();
            _dashButton.DeActivate();
        }

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