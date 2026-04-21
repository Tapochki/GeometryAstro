using System;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Fires extra shots with a short delay between each duplicate.
    /// extraDuplicates = 0  → 1 shot per trigger
    /// extraDuplicates = 1  → 2 shots, etc.
    /// </summary>
    public class DuplicatorComponent : ITickable
    {
        private const float DuplicatorDelay = 0.2f;

        private int _extraDuplicates;
        private readonly Action _onShoot;
        private readonly Action _onEnd;

        private int _shotsFired;
        private float _timer;
        private bool _isActive;

        public DuplicatorComponent(Action onShoot, Action onEnd, int extraDuplicates = 0)
        {
            _onShoot = onShoot;
            _onEnd = onEnd;
            _extraDuplicates = extraDuplicates;
        }

        /// <summary>Starts the duplicator sequence.</summary>
        public void Activate()
        {
            _shotsFired = 0;
            ScheduleNext();
        }

        public void Tick()
        {
            if (!_isActive) return;

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
                FireNext();
        }

        public void UpgradeDuplicateCount() => _extraDuplicates++;

        private void ScheduleNext()
        {
            _timer = DuplicatorDelay;
            _isActive = true;
        }

        private void FireNext()
        {
            _isActive = false;
            _onShoot?.Invoke();
            _shotsFired++;

            if (_shotsFired > _extraDuplicates)
                _onEnd?.Invoke();
            else
                ScheduleNext();
        }
    }
}
