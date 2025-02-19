using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Transform _bodyTransform;

        private IGameplayInputHandler _inputHandler;
        private EventBusHolder _eventBusHolder;

        private PlayerConfig _playerConfig;

        private IMove _moveComponent;
        private IRotation _mainRotateComponent;
        private HealthComponent _healthComponent;

        public Vector2 PlayerPosition { get => transform.position; }

        private Action<float, float> _onHealthChageEvent;
        private Action _onPlayerDieEvent;

        private PlayerHealthChangeEvent _playerHealthChangeEvent;

        private PlayerDieEvent _playerDieEvent;

        private void Construct(IGameplayInputHandler inputHandler, PlayerConfig playerConfig, EventBusHolder eventBusHolder)
        {
            _inputHandler = inputHandler;
            _eventBusHolder = eventBusHolder;
            _playerConfig = playerConfig;
        }

        private void Start()
        {
            _moveSpeed = _playerConfig.PlayerData.StartSpeed;
            _onHealthChageEvent += (currentHealth, maxHealth) => _eventBusHolder.EventBus.Raise(new PlayerHealthChangeEvent(currentHealth, maxHealth));
            _onPlayerDieEvent += () => _eventBusHolder.EventBus.Raise(new PlayerDieEvent());

            _moveComponent = new MoveComponent(gameObject.GetComponent<Rigidbody2D>());
            _mainRotateComponent = new PlayerRotateComponent(_bodyTransform);
            _healthComponent = new HealedHealthComponent(_playerConfig.PlayerData.StartHealth, _onPlayerDieEvent, _onHealthChageEvent);

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