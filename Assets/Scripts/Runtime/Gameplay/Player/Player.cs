using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class Player : MonoBehaviour
    {    
        [SerializeField] private Transform _bodyTransform;

        private float _moveSpeed;
        private IGameplayInputHandler _inputHandler;
        private EventBusHolder _eventBusHolder;

        private PlayerData _playerData;

        private IMove _moveComponent;
        private IRotation _mainRotateComponent;
        private HealthComponent _healthComponent;

        public Vector2 PlayerPosition { get => transform.position; }

        private Action<float, float> _onHealthChageEvent;
        private Action _onPlayerDieEvent;

        private PlayerHealthChangeEvent _playerHealthChangeEvent;

        private PlayerDieEvent _playerDieEvent;

        [Inject]
        private void Construct(IGameplayInputHandler inputHandler, EventBusHolder eventBusHolder)
        {
            _inputHandler = inputHandler;
            _eventBusHolder = eventBusHolder;
        }

        public void Init(PlayerData playerData)
        {
            _playerData = playerData;
            _moveSpeed = _playerData.StartSpeed;
            _onHealthChageEvent += (currentHealth, maxHealth) => _eventBusHolder.EventBus.Raise(new PlayerHealthChangeEvent(currentHealth, maxHealth));
            _onPlayerDieEvent += () => _eventBusHolder.EventBus.Raise(new PlayerDieEvent());

            _moveComponent = new MoveComponent(gameObject.GetComponent<Rigidbody2D>());
            _mainRotateComponent = new PlayerRotateComponent(_bodyTransform);
            _healthComponent = new HealedHealthComponent(_playerData.StartHealth, _onPlayerDieEvent, _onHealthChageEvent);
        }

        private void FixedUpdate()
        {
            _moveComponent.Move(_inputHandler.MoveDirection, _moveSpeed);
            if (_inputHandler.RotationDirection != Vector2.zero)
            {
                _mainRotateComponent.SetRotation(_inputHandler.RotationDirection);
            }
            else if (_inputHandler.MoveDirection != Vector2.zero)
            {
                _mainRotateComponent.SetRotation(_inputHandler.MoveDirection);
            }

            if(_mainRotateComponent != null) 
            {
                _mainRotateComponent.Update();
            }
        }

        public void TakeDamage(float damage)
        {
            _healthComponent.TakeDamage(damage);
        }
    }
}