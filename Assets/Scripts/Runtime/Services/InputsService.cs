using System;
using UnityEngine;

namespace TandC.GeometryAstro.Services
{
    public class InputsSystem : MonoBehaviour
    {
        public event Action OnMovementDirectionUpdatedEvent;

        public event Action OnEscapeButtonDownEvent;

        private GameStateSystem _gameStateSystem;

        public void Construct(GameStateSystem gameStateSystem)
        {
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