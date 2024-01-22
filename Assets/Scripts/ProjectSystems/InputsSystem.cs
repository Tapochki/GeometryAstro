using ChebDoorStudio.Settings;
using ChebDoorStudio.Utilities;
using System;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.ProjectSystems
{
    public class InputsSystem : IInitializable
    {
        public event Action OnMovementDirectionUpdatedEvent;

        public event Action OnEscapeButtonDownEvent;

        private GameStateSystem _gameStateSystem;
        private MonoHelper _monoHelper;

        [Inject]
        public void Construct(GameStateSystem gameStateSystem, MonoHelper monoHelper)
        {
            Utilities.Logger.Log("InputsSystem Construct", LogTypes.Info);

            _gameStateSystem = gameStateSystem;
            _monoHelper = monoHelper;

            _monoHelper.OnUpdateEvent += OnUpdateEventHandler;
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