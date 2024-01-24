using System;
using UnityEngine;
using TandC.EventBus;

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

        public Vector2 PlayerPosition { get => transform.position; }

        private Action<float> _onHealthChageEvent;
        public Action onPlayerDieEvent;

        private void Start()
        {
            _moveComponent = new MoveComponent(gameObject.GetComponent<Rigidbody2D>());
            _mainRotateComponent = new PlayerRotateComponent(_bodyTransform);
            _healthComponent = new PlayerHealthComponent(100f, onPlayerDieEvent, _onHealthChageEvent);

            onPlayerDieEvent += () => _eventBusHolder.EventBus.Raise(new PlayerDieEvent());
            _onHealthChageEvent += value => _eventBusHolder.EventBus.Raise(new PlayerHealthChangeEvent(value));
        }

        private void FixedUpdate()
        {
            _moveComponent.Move(_inputHandler.MoveDirection, _moveSpeed);
            if(_inputHandler.RotationDirection != Vector2.zero) 
            {
                _mainRotateComponent.Rotation(_inputHandler.RotationDirection);
            }
            else if(_inputHandler.MoveDirection != Vector2.zero) 
            {
                _mainRotateComponent.Rotation(_inputHandler.MoveDirection);
            }
        }
    }
}

