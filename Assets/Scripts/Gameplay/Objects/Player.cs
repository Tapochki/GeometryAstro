using System;
using TandC.Data;
using TandC.EventBus;
using UnityEngine;

namespace TandC.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private EventBusHolder _eventBusHolder;
        [SerializeField] private Transform _bodyTransform;

        private IMove _moveComponent;
        private IRotation _mainRotateComponent;
        private HealthComponent _healthComponent;

        private PlayerData _playerData;

        public Vector2 PlayerPosition { get => transform.position; }

        private Action<float, float> _onHealthChageEvent;
        private Action _onPlayerDieEvent;

        private void Start()
        {
            _moveComponent = new MoveComponent(gameObject.GetComponent<Rigidbody2D>());
            _mainRotateComponent = new PlayerRotateComponent(_bodyTransform);
            _healthComponent = new HealedHealthComponent(100f, _onPlayerDieEvent, _onHealthChageEvent);

            _onPlayerDieEvent += () => _eventBusHolder.EventBus.Raise(new PlayerDieEvent());
            _onHealthChageEvent += (currentHealth, maxHealth) => _eventBusHolder.EventBus.Raise(new PlayerHealthChangeEvent(currentHealth, maxHealth));
        }

        private void FixedUpdate()
        {
            _moveComponent.Move(_inputHandler.MoveDirection, _moveSpeed);
            if (_inputHandler.RotationDirection != Vector2.zero)
            {
                _mainRotateComponent.Rotation(_inputHandler.RotationDirection);
            }
            else if (_inputHandler.MoveDirection != Vector2.zero)
            {
                _mainRotateComponent.Rotation(_inputHandler.MoveDirection);
            }
        }

        public void TakeDamage(float damage)
        {
            _healthComponent.TakeDamage(damage);
        }
    }
}