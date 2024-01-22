using ChebDoorStudio.Settings;
using System;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.ProjectSystems
{
    public class InputsSystem : MonoBehaviour
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

        public void Update()
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