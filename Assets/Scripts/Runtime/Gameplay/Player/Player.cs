using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Services;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class Player : MonoBehaviour
    {    
        [SerializeField] private Transform _bodyTransform;

        private float _moveSpeed;
        private IGameplayInputHandler _inputHandler;
        private TickService _tickService;

        private PlayerData _playerData;

        private IMove _moveComponent;
        private IRotation _mainRotateComponent;
        private HealthComponent _healthComponent;

        public Vector2 PlayerPosition { get => transform.position; }

        private Action<float, float> _onHealthChageEvent;
        private Action<bool> _onPlayerDieEvent;

        private PlayerDieEvent _playerDieEvent;

        private LevelModel _levelModel;

        [Inject]
        private void Construct(IGameplayInputHandler inputHandler, TickService tickService)
        {
            _inputHandler = inputHandler;
            _tickService = tickService;
        }

        public void Init(PlayerData playerData)
        {
            _levelModel = new LevelModel();
            _levelModel.Init();
            _playerData = playerData;
            _moveSpeed = _playerData.StartSpeed;
            _onHealthChageEvent += (currentHealth, maxHealth) => EventBusHolder.EventBus.Raise(new PlayerHealthChangeEvent(currentHealth, maxHealth));
            _onPlayerDieEvent += (isKilled) => EventBusHolder.EventBus.Raise(new PlayerDieEvent());

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