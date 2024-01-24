using System;
using UnityEngine;


namespace TandC.Gameplay 
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Joystick _joystick;

        private MoveComponent _moveComponent;
        private RotateComponent _rotateComponent;
        private HealthComponent _healthComponent;

        public Vector2 PlayerPosition { get => transform.position; }

        private Action<float> _onHealthChageEvent;
        public Action onPlayerDieEvent;

        private void Start()
        {
            _moveComponent = new MoveComponent(gameObject.GetComponent<Rigidbody2D>());
            _rotateComponent = new RotateComponent(transform);
            _healthComponent = new PlayerHealthComponent(100f, onPlayerDieEvent, _onHealthChageEvent);
        }

        private void FixedUpdate()
        {
            _moveComponent.Move(_joystick.Direction, _moveSpeed);

            if (_joystick.Direction != Vector2.zero)
            {
                _rotateComponent.RotateTowards(_joystick.Direction);
            }
            else
            {
                _rotateComponent.SaveLastRotation();
            }
        }
    }
}

