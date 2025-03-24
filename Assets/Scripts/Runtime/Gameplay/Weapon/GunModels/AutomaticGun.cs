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

        private DuplicatorComponent _duplicatorComponent;

        private bool _shootStart;

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

        public void RegisterDuplicatorComponent(IReadableModificator duplicateModificator)
        {
            _duplicatorComponent = new DuplicatorComponent(duplicateModificator, TryShoot, EndShoot);
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

        private void ShootAction() 
        {
            _shootStart = true;
            _duplicatorComponent.Activate();
        }

        private void TryShoot()
        {
            Vector2? target = _enemyDetector.GetEnemyPosition(_weaponShootingPattern.Origin.position, default, _data.detectorDistance);
            if (target.HasValue)
            {
                Shoot(_weaponShootingPattern.Origin.position, target.Value);
            }
        }

        private void EndShoot() 
        {
            _shootStart = false;
            _reloader.StartReload();
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
            _duplicatorComponent?.Tick();
            _reloader.Update();
            if (_reloader.CanShoot && !_shootStart)
            {
                ShootAction();
            }
        }
    }
}
