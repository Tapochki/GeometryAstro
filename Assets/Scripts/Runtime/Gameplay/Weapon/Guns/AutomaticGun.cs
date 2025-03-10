using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class AutomaticGun : MonoBehaviour, IWeapon, IDisposable
    {
        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private WeaponData _data;
        private CompositeDisposable _disposables = new();

        private WeaponShootingPattern _weaponShootingPattern;

        [SerializeField] private Transform _bulletParent;
        [SerializeField] private WeaponConfig _config;

        private int _currentLevel = 1;

        private void Start()
        {
            _data = _config.GetWeaponByType(WeaponType.AutoGun);
            _projectileFactory = new ProjectileFactory(_data.bulletData, _bulletParent, 10);
            _reloader = new WeaponReloader(_data.shootDeley);
            _enemyDetector = new CircleEnemyDetector(LayerMask.GetMask("Enemy"));
            RegisterShootingPatterns();
        }

        private void RegisterShootingPatterns()
        {
            foreach (var pattern in FindObjectsOfType<WeaponShootingPattern>())
            {
                if (pattern.Type == WeaponType.AutoGun)
                {
                    _weaponShootingPattern = pattern;
                    break;
                }
            }
        }

        private void TryShoot()
        {
            Vector2? target = _enemyDetector.GetEnemyPosition(_weaponShootingPattern.Origin.position, default, _data.detectorDistance);
            if (target.HasValue)
            {
                Shoot(_weaponShootingPattern.Origin.position, target.Value);
                _reloader.StartReload();
            }
        }

        private void Shoot(Vector2 origin, Vector2 direction)
        {
            _projectileFactory.CreateProjectile(
                origin,
                direction,
                _data.baseDamage
            );
        }

        public void Update()
        {
            _reloader.Update();
            if (_reloader.CanShoot)
            {
                TryShoot();
            }
        }

        public void Dispose() => _disposables.Dispose();

        public void UpdateWeapon(float deltaTime)
        {

        }

        public void Upgrade()
        {

        }
    }
}
