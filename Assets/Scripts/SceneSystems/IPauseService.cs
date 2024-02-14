using System;
using UnityEngine;
using Zenject;

namespace TandC.SceneSystems
{
    public interface IPauseService
    {
        public event Action<bool> OnGameplayPausedEvent;

        public bool IsPaused { get; }

        public void SetOn();

        public void SetOff();
    }
}