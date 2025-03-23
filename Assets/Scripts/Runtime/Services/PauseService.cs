using System;
using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Services
{
    public class PauseService : IPauseService, IEventReceiver<PauseGameEvent>
    {
        public UniqueId Id { get; } = new UniqueId();

        public event Action<bool> OnGameplayPausedEvent;

        public bool IsPaused { get; private set; }

        public void Init() 
        {
            RegisterEvent();
            SetOff();
        }

        public void Dispose() 
        {
            UnregisterEvent();
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<PauseGameEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<PauseGameEvent>);
        }

        public void OnEvent(PauseGameEvent @event)
        {
            if (@event.SetPause)
            {
                SetOn();
            }
            else
            {
                SetOff();
            }
        }

        public void SetOn()
        {
            IsPaused = true;
            Time.timeScale = 0.0f;
            OnGameplayPausedEvent?.Invoke(IsPaused);
        }

        public void SetOff()
        {
            IsPaused = false;
            Time.timeScale = 1.0f;
            OnGameplayPausedEvent?.Invoke(IsPaused);
        }
    }
}