using System;
using TandC.Data;
using TandC.EventBus;
using UnityEngine;
using Zenject;

namespace TandC.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Transform _bodyTransform;

        private InputHandler _inputHandler;
        private EventBusHolder _eventBusHolder;

        private IMove _moveComponent;
        private IRotation _mainRotateComponent;
        private HealthComponent _healthComponent;

        private PlayerData _playerData;

        public Vector2 PlayerPosition { get => transform.position; }

        private Action<float, float> _onHealthChageEvent;
        private Action _onPlayerDieEvent;

        [Inject]
        private void Construct(InputHandler inputHandler, EventBusHolder eventBusHolder)
        {
            _inputHandler = inputHandler;
            _eventBusHolder = eventBusHolder;
        }

        private void Start()
        {
            _onHealthChageEvent += (currentHealth, maxHealth) => _eventBusHolder.EventBus.Raise(new PlayerHealthChangeEvent(currentHealth, maxHealth));
            _onPlayerDieEvent += () => _eventBusHolder.EventBus.Raise(new PlayerDieEvent());

            _moveComponent = new MoveComponent(gameObject.GetComponent<Rigidbody2D>());
            _mainRotateComponent = new PlayerRotateComponent(_bodyTransform);
            _healthComponent = new HealedHealthComponent(100f, _onPlayerDieEvent, _onHealthChageEvent);

        }

        private void UpdatePlayerHealth(float maxValue, float minValue) 
        {
            Debug.LogError(maxValue + minValue);
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

            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                _healthComponent.TakeDamage(10);
            }
        }

        public void TakeDamage(float damage)
        {
            _healthComponent.TakeDamage(damage);
        }
    }
}