using System;
using TandC.Settings;
using UnityEngine;
using Zenject;

namespace TandC.ProjectSystems
{
    public class GameStateSystem : MonoBehaviour
    {
        public event Action<GameStates> OnGameStateWasChangedEvent;

        public event Action OnGameplayStartedEvent;

        public event Action OnGameplayStopedEvent;

        public GameStates CurrentState { get; private set; } = GameStates.Unknown;

        public bool GameStarted { get; private set; } = false;
        public bool WorkerSceneInitialized { get; private set; } = false;

        [Inject]
        public void Construct()
        {
            Utilities.Logger.Log("GameStateSystem Construct", LogTypes.Info);
        }

        public void Initialize()
        {
        }

        public void ChangeGameState(GameStates targetState)
        {
            if (CurrentState == targetState || targetState == GameStates.Unknown)
            {
                return;
            }

            CurrentState = targetState;
            OnGameStateWasChangedEvent?.Invoke(CurrentState);
        }

        public void WorkerInitialized()
        {
            WorkerSceneInitialized = true;
        }

        public void WorkerUninitialized()
        {
            WorkerSceneInitialized = false;
        }

        public void GameplayStarted()
        {
            GameStarted = true;

            OnGameplayStartedEvent?.Invoke();
        }

        public void GameplayStoped()
        {
            GameStarted = false;

            OnGameplayStopedEvent?.Invoke();
        }
    }
}