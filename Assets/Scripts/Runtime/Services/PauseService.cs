using System;
using UnityEngine;

namespace TandC.GeometryAstro.Services
{
    public class PauseService : MonoBehaviour, IPauseService
    {
        public event Action<bool> OnGameplayPausedEvent;

        public bool IsPaused { get; private set; }

        public void SetOn()
        {
            if (IsPaused)
            {
                return;
            }

            IsPaused = true;
            Time.timeScale = 0.0f;
            OnGameplayPausedEvent?.Invoke(IsPaused);
        }

        public void SetOff()
        {
            if (!IsPaused)
            {
                return;
            }

            IsPaused = false;
            Time.timeScale = 1.0f;
            OnGameplayPausedEvent?.Invoke(IsPaused);
        }
    }
}