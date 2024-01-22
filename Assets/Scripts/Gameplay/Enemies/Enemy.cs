using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.Gameplay.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private Transform _selfTransform;

        private EnemyMovement _movement;

        private GameStateSystem _gameStateSystem;
        private InitialGameData _initialGameData;

        private Vector3 _initialRotation;

        [Inject]
        public void Construct(GameStateSystem gameStateSystem, InitialGameData initialGameData)
        {
            _gameStateSystem = gameStateSystem;
            _initialGameData = initialGameData;

            _gameStateSystem.OnGameplayStopedEvent += OnGameplayStopedEventHandler;
        }

        [Inject]
        public void Initialize()
        {
            _selfTransform = this.transform;

            _movement = new EnemyMovement(_selfTransform, _initialGameData);

            _initialRotation = _selfTransform.localEulerAngles;
        }

        private void Update()
        {
            if (!_gameStateSystem.GameStarted)
            {
                return;
            }

            _movement.Update();
        }

        private void FixedUpdate()
        {
            if (!_gameStateSystem.GameStarted)
            {
                return;
            }

            _movement.FixedUpdate();
        }

        private void OnGameplayStopedEventHandler()
        {
            _selfTransform.localEulerAngles = _initialRotation;
            _movement.Reset();
        }
    }

    public sealed class EnemyMovement
    {
        private Transform _selfTransform;

        private float _minRotatingSpeed;
        private float _maxRotatingSpeed;
        private float _currentRotatingSpeed;

        private float _minRotateTime;
        private float _maxRotateTime;
        private float _currentRotateTime;

        private float _calculatedRotateTime;

        private Vector3 _rotation;

        private InitialGameData _initialGameData;

        public EnemyMovement(Transform selfTransform, InitialGameData initialGameData)
        {
            _selfTransform = selfTransform;
            _initialGameData = initialGameData;

            ResetRotation();

            _minRotatingSpeed = _initialGameData.enemyData.minRotatingSpeed;
            _maxRotatingSpeed = _initialGameData.enemyData.maxRotatingSpeed;

            _minRotateTime = _initialGameData.enemyData.minRotatingTime;
            _maxRotateTime = _initialGameData.enemyData.maxRotatingTime;

            CalculateRotateTime();
            CalculateRotateSpeed();
        }

        private void ResetRotation()
        {
            _rotation = Vector3.forward;
        }

        public void Reset()
        {
            _selfTransform.localEulerAngles = _initialGameData.enemyData.initialRotate;

            CalculateRotateTime();
            CalculateRotateSpeed();
        }

        public void Update()
        {
            _currentRotateTime += Time.deltaTime;

            if (_currentRotateTime > _calculatedRotateTime)
            {
                CalculateRotateTime();
                CalculateRotateSpeed();
            }
        }

        private void CalculateRotateSpeed()
        {
            _currentRotatingSpeed = _minRotatingSpeed + (_maxRotatingSpeed - _minRotatingSpeed) * 0.1f * Random.Range(0, 11);
            _currentRotatingSpeed *= Random.Range(0, 2) == 0 ? 1f : -1f;

            ResetRotation();
            _rotation *= _currentRotatingSpeed;
        }

        private void CalculateRotateTime()
        {
            _currentRotateTime = 0f;
            _calculatedRotateTime = _minRotateTime + (_maxRotateTime - _minRotateTime) * 0.1f * Random.Range(0, 11);
        }

        public void FixedUpdate()
        {
            _selfTransform.Rotate(_rotation * Time.fixedDeltaTime);
        }
    }
}