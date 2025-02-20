using System;
using UnityEngine;

namespace TandC.GeometryAstro.Services
{
    public interface IPauseService
    {
        public event Action<bool> OnGameplayPausedEvent;

        public bool IsPaused { get; }

        public void SetOn();

        public void SetOff();
    }
}