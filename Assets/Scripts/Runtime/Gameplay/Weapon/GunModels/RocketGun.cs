using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RocketGun : IWeapon
    {
        private RocketAmmo _rocketAmmo;
        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private WeaponData _data;

        private WeaponShootingPattern _weaponShootingPattern;

        public WeaponType WeaponType { get; private set; }

        private int _currentLevel = 1;

        public void SetData(WeaponData data)
        {
            _data = data;
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
        }

        public void Initialization()
        {
            InitRocketAmmo();
            InitRocketButton();
            RegisterShootingPatterns();
        }

        private void InitRocketButton() 
        {
            RocketInputButton reloadButton = GameObject.FindAnyObjectByType<RocketInputButton>();

            reloadButton.Initialize(_reloader.ReloadProgress, _rocketAmmo.RocketCount, TryShoot);
        }

        private void InitRocketAmmo()
        {
            _rocketAmmo = new RocketAmmo(10);
        }

        private void RegisterShootingPatterns()
        {
            foreach (var pattern in GameObject.FindObjectsOfType<WeaponShootingPattern>())
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

        public void Upgrade()
        {

        }

        public void Tick()
        {
            _reloader.Update();
        }
    }
}
