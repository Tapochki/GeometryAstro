using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class SkillReloader : IReloadable
    {
        public IReadOnlyReactiveProperty<float> ReloadProgress => _reloadProgress;
        private IReadableModificator _reloadModificator;

        private ReactiveProperty<float> _reloadProgress = new ReactiveProperty<float>(0f);

        public bool CanAction { get; private set; }

        private float _reloadTimer;
        private float _reloadTime;
        private bool _isReloading;

        public SkillReloader(float reloadTime, IReadableModificator ReloadModificator)
        {
            _reloadModificator = ReloadModificator;
            _reloadTime = reloadTime;
            CanAction = true;
            _isReloading = false;
            _reloadProgress.Value = 1f;
        }

        public void StartReload()
        {
            CanAction = false;
            _reloadTimer = _reloadTime * _reloadModificator.Value;
            _isReloading = true;
            _reloadProgress.Value = 0f;
        }

        public void Update()
        {
            if (!_isReloading) return;

            _reloadTimer -= Time.deltaTime;
            _reloadProgress.Value = Mathf.Clamp01(1f - (_reloadTimer / _reloadTime));

            if (_reloadTimer <= 0)
            {
                _isReloading = false;
                CanAction = true;
            }
        }
    }
}
