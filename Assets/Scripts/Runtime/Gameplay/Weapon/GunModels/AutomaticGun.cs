using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class AutomaticGun : IWeapon
    {
        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private WeaponData _data;

        private WeaponShootingPattern _weaponShootingPattern;

        private int _currentLevel = 1;

        public WeaponType WeaponType { get; private set; }

        public void SetData(WeaponData data) 
        {
            _data = data;
            WeaponType = _data.type;
        }

        public void SetProjectileFactory(IProjectileFactory projectileFactory) 
        {
            _projectileFactory = projectileFactory;
        }

        public void SetReloader(IReloadable reloader)
        {
            _reloader = reloader;
        }

        public void SetEnemyDetector(IEnemyDetector enemyDetector)
        {
            _enemyDetector = enemyDetector;
        }

        public void Initialization() 
        {
            RegisterShootingPatterns();
        }

        private void RegisterShootingPatterns()
        {
            foreach (var pattern in MonoBehaviour.FindObjectsOfType<WeaponShootingPattern>())
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
                direction
            );
        }

        public void Upgrade()
        {

        }

        public void Tick()
        {
            _reloader.Update();
            if (_reloader.CanShoot)
            {
                TryShoot();
            }
        }
    }
}
