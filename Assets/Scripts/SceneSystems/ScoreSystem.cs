using ChebDoorStudio.Gameplay.Player;
using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.ScriptableObjects;
using System;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.SceneSystems
{
    public class ScoreSystem : MonoBehaviour
    {
        public event Action<int> OnScoreUpdateEvent;

        public event Action<int> OnBestScoreChangedEvent;

        private int _bestScore;
        private float _currentScore;

        private float _scoreMultiplier;

        private int _currentLevel;

        private bool _isStartCalculation;

        private PlayerComponent _player;
        private GameStateSystem _gameStateSystem;
        private DataSystem _dataSystem;
        private InitialGameData _initialGameData;

        [Inject]
        public void Construct(PlayerComponent playerComponent, GameStateSystem gameStateSystem,
                                DataSystem dataSystem, InitialGameData initialGameData)
        {
            _player = playerComponent;
            _gameStateSystem = gameStateSystem;
            _dataSystem = dataSystem;
            _initialGameData = initialGameData;

            _player.OnPlayerDeathEvent += OnPlayerDeathEventHandler;
            _gameStateSystem.OnGameplayStopedEvent += OnGameplayStopedEventHandler;
            _gameStateSystem.OnGameplayStartedEvent += OnGameplayStartedEventHandler;
        }

        public int GetScore() => (int)_currentScore;

        private void Update()
        {
            if (_isStartCalculation)
            {
                _currentScore += _scoreMultiplier * Time.deltaTime;

                OnScoreUpdateEvent?.Invoke((int)_currentScore);

                if (_currentLevel > _initialGameData.levels.Count)
                {
                    _currentLevel = 0;
                }

                if (_currentScore > _initialGameData.levels[_currentLevel].level)
                {
                    _currentLevel++;
                    _scoreMultiplier = _initialGameData.scoresMultipliers[_currentLevel].scoreMultiplier;
                }
            }
        }

        public void Dispose()
        {
            _player.OnPlayerDeathEvent -= OnPlayerDeathEventHandler;
            _gameStateSystem.OnGameplayStopedEvent -= OnGameplayStopedEventHandler;
            _gameStateSystem.OnGameplayStartedEvent -= OnGameplayStartedEventHandler;

            _player = null;
            _gameStateSystem = null;
            _dataSystem = null;
            _initialGameData = null;
        }

        private void OnPlayerDeathEventHandler()
        {
            _isStartCalculation = false;

            if (_bestScore < (int)_currentScore)
            {
                _bestScore = (int)_currentScore;

                _dataSystem.PlayerVaultData.bestScore = _bestScore;

                _dataSystem.SaveCache(Settings.CacheType.PlayerValutData);
                OnBestScoreChangedEvent?.Invoke(_bestScore);
            }
        }

        private void OnGameplayStopedEventHandler()
        {
            ResetInitialData();
        }

        private void OnGameplayStartedEventHandler()
        {
            ResetInitialData();

            _isStartCalculation = true;
        }

        private void ResetInitialData()
        {
            _bestScore = _dataSystem.PlayerVaultData.bestScore;

            _currentScore = 0.0f;
            _currentLevel = 0;

            _scoreMultiplier = _initialGameData.scoresMultipliers[_currentLevel].scoreMultiplier;

            OnScoreUpdateEvent?.Invoke((int)_currentScore);
        }
    }
}