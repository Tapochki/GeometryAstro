using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RocketGun : MonoBehaviour, IWeapon, IDisposable
    {
        [SerializeField] private RocketInputButton _reloadButton;

        [SerializeField] private Transform _bulletParent;
        [SerializeField] private WeaponConfig _config;

        private RocketAmmo _rocketAmmo;
        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private WeaponData _data;
        private CompositeDisposable _disposables = new();

        private WeaponShootingPattern _weaponShootingPattern;

        private int _currentLevel = 1;

        private void Start()
        {
            _data = _config.GetWeaponByType(WeaponType.RocketGun);
            _projectileFactory = new ProjectileFactory(_data.bulletData, _bulletParent, 2);
            _reloader = new WeaponReloader(_data.shootDeley);
            _enemyDetector = new CircleEnemyDetector(LayerMask.GetMask("Enemy"));
            _rocketAmmo = new RocketAmmo(10);
            _reloadButton.Initialize(_reloader.ReloadProgress, _rocketAmmo.RocketCount, TryShoot);
            RegisterShootingPatterns();
        }

        private void RegisterShootingPatterns()
        {
            foreach (var pattern in FindObjectsOfType<WeaponShootingPattern>())
            {
                if (pattern.Type == WeaponType.RocketGun)
                {
                    _weaponShootingPattern = pattern;
                    break;
                }
            }
        }

        private void TryShoot()
        {
            if (_rocketAmmo.TryShoot())
            {
                Shoot(_weaponShootingPattern.Origin.position, _weaponShootingPattern.Direction.position);
                _reloader.StartReload();
            }
            else
            {
                Debug.Log("No Rockets Left!");
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
