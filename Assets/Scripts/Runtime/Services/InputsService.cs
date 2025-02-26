using System;
using UnityEngine;

namespace TandC.GeometryAstro.Services
{
    public class InputsService : MonoBehaviour
    {
        public event Action OnMovementDirectionUpdatedEvent;

        public event Action OnEscapeButtonDownEvent;

        private GameStateService _gameStateService;

        public void Construct(GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        public void Initialize()
        {
        }

        public void Update()
        {
            if (_gameStateService.WorkerSceneInitialized)
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    OnEscapeButtonDownEvent?.Invoke();
                }
            }

            if (_gameStateService.GameStarted)
            {
            }
        }

        public void UpdateMovementDirection()
        {
            OnMovementDirectionUpdatedEvent?.Invoke();
        }
    }
}