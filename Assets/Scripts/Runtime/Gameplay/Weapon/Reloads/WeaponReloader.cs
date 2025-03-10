using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class WeaponReloader : IReloadable
    {
        public IReadOnlyReactiveProperty<float> ReloadProgress => _reloadProgress;

        private ReactiveProperty<float> _reloadProgress = new ReactiveProperty<float>(0f);

        public bool CanShoot { get; private set; }

        private float _reloadTimer;
        private float _reloadTime;
        private bool _isReloading;

        public WeaponReloader(float reloadTime)
        {
            _reloadTime = reloadTime;
            CanShoot = true;
            _isReloading = false;
            _reloadProgress.Value = 1f;
        }

        public void StartReload()
        {
            CanShoot = false;
            _reloadTimer = _reloadTime;
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
                CanShoot = true;
            }
        }
    }
}
