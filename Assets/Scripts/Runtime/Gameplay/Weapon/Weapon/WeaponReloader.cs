using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class WeaponReloader : IReloadable
    {
        public float ReloadProgress => Mathf.Clamp01(_reloadTimer / _reloadTime);
        public bool CanShoot { get; private set; }

        private float _reloadTimer;
        private float _reloadTime;
        private bool _isReloading;

        public WeaponReloader(float reloadTime)
        {
            _reloadTime = reloadTime;
            CanShoot = false;
            _isReloading = true;
        }

        public void StartReload()
        {
            CanShoot = false;
            _reloadTimer = _reloadTime;
            _isReloading = true;
        }

        public void Update()
        {
            if (!_isReloading) return;

            _reloadTimer -= Time.deltaTime;

            if (_reloadTimer <= 0)
            {
                _isReloading = false;
                CanShoot = true;
            }
        }
    }
}
