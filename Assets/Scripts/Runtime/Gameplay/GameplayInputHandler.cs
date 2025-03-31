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
        [SerializeField] private SkillInputButton _laserButton;

        public Vector2 MoveDirection { get; private set; }
        public Vector2 RotationDirection { get; private set; }

        public RocketInputButton RocketButton => _rocketButton;
        public SkillInputButton CloakButton => _cloakButton;
        public SkillInputButton DashButton => _dashButton;
        public SkillInputButton LaserButton => _laserButton;

        private bool _isCanMove;

        public void Init() 
        {
            _rocketButton.DeActivate();
            _cloakButton.DeActivate();
            _dashButton.DeActivate();
            _laserButton.DeActivate();
            _isCanMove = true;
        }

        private void FixedUpdate()
        {
#if UNITY_EDITOR
            SimulateInputInEditor();
#else
            UpdateInputFromJoysticks();
#endif
        }

        public void SetInteractable(bool value) 
        {
            _isCanMove = value;
            if (!value) 
            {
                MoveDirection = Vector2.zero;
                RotationDirection = Vector2.zero;
            }
        }

        private void SimulateInputInEditor()
        {
            if (!_isCanMove)
                return;
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