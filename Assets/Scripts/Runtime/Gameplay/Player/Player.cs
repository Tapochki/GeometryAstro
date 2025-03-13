using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Services;
using UnityEditor.Experimental.GraphView;
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
        private TickService _tickService;

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
        private void Construct(IGameplayInputHandler inputHandler, EventBusHolder eventBusHolder, TickService tickService)
        {
            _inputHandler = inputHandler;
            _eventBusHolder = eventBusHolder;
            _tickService = tickService;
            
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

            _tickService.RegisterFixedUpdate(FixedTick);
        }

        private void FixedTick()
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