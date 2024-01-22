using Studio.Settings;
using Studio.Utilities;
using System;
using UnityEngine;
using Zenject;

namespace Studio.ProjectSystems
{
    public class InputsSystem : IInitializable
    {
        public event Action OnMovementDirectionUpdatedEvent;

        public event Action OnEscapeButtonDownEvent;

        private GameStateSystem _gameStateSystem;

        [Inject]
        public void Construct(GameStateSystem gameStateSystem)
        {
            Utilities.Logger.Log("InputsSystem Construct", LogTypes.Info);

            _gameStateSystem = gameStateSystem;
        }

        public void Initialize()
        {
        }

        public void OnUpdateEventHandler()
        {
            if (_gameStateSystem.WorkerSceneInitialized)
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    OnEscapeButtonDownEvent?.Invoke();
                }
            }

            if (_gameStateSystem.GameStarted)
            {
            }
        }

        public void UpdateMovementDirection()
        {
            OnMovementDirectionUpdatedEvent?.Invoke();
        }
    }
}