using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Countdown-based reload timer. No external dependencies.
    /// </summary>
    public class SkillReloader : IReloadable
    {
        public float ReloadProgress { get; private set; }
        public bool CanAction { get; private set; }

        private readonly float _baseReloadTime;
        private readonly float _reloadMultiplier;

        private float _reloadTimer;
        private bool _isReloading;

        /// <param name="reloadTime">Base seconds to reload.</param>
        /// <param name="reloadMultiplier">Multiplier for upgrade scaling (default 1 = no change).</param>
        public SkillReloader(float reloadTime, float reloadMultiplier = 1f)
        {
            _baseReloadTime = reloadTime;
            _reloadMultiplier = reloadMultiplier;
            CanAction = true;
            ReloadProgress = 1f;
        }

        public void StartReload()
        {
            CanAction = false;
            _reloadTimer = _baseReloadTime * _reloadMultiplier;
            _isReloading = true;
            ReloadProgress = 0f;
        }

        public void Update()
        {
            if (!_isReloading) return;

            _reloadTimer -= Time.deltaTime;
            float total = _baseReloadTime * _reloadMultiplier;
            ReloadProgress = Mathf.Clamp01(1f - (_reloadTimer / total));

            if (_reloadTimer <= 0f)
            {
                _isReloading = false;
                CanAction = true;
                ReloadProgress = 1f;
            }
        }
    }
}
