using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RocketGun : IActiveSkill
    {
        private RocketAmmo _rocketAmmo;
        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private ActiveSkillData _data;

        private WeaponShootingPattern _weaponShootingPattern;

        private DuplicatorComponent _duplicatorComponent;

        private IPassiveUpgradable _explosionRadiusModificatorUpgrade;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.RocketGun;

        private int _currentLevel = 1;

        private bool _shootStart;

        public void SetData(ActiveSkillData data)
        {
            _data = data;
            InitRadiusModificator();
        }

        public void SetProjectileFactory(IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplierModificator,
            IReadableModificator bulletSizeModificator, int startBulletPreloadCount)
        {
            _projectileFactory = new ProjectileFactory(
                _data.bulletData,
                startBulletPreloadCount,
                () => Object.Instantiate(_data.bulletData.BulletObject).GetComponent<ExplosiveBullet>().SetExplosiveDamageAreaBullet((IReadableModificator)_explosionRadiusModificatorUpgrade),
                damageModificator,
                criticalChanceModificator,
                criticalDamageMultiplierModificator,
                bulletSizeModificator);
        }

        public void SetReloader(IReloadable reloader, RocketInputButton rocketInputButton)
        {
            _reloader = reloader;
            rocketInputButton.Initialize(_reloader.ReloadProgress, _rocketAmmo.RocketCount, _rocketAmmo.MaxRocketCount, ShootAction);
        }

        public void SetEnemyDetector(IEnemyDetector enemyDetector)
        {
        }

        public void Initialization()
        {
            RegisterShootingPatterns();
        }

        private void InitRadiusModificator() 
        {
            _explosionRadiusModificatorUpgrade = new Modificator(_data.detectorRadius, 0, true);
        }

        public void InitRocketAmmo()
        {
            _rocketAmmo = new RocketAmmo(10);
        }

        public void RegisterDuplicatorComponent(IReadableModificator duplicateModificator)
        {
            _duplicatorComponent = new DuplicatorComponent(duplicateModificator, TryShoot, EndShoot);
        }

        private void RegisterShootingPatterns()
        {
            foreach (var pattern in GameObject.FindObjectsOfType<WeaponShootingPattern>())
            {
                if (pattern.Type == SkillType)
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
            }
            else
            {
                Debug.Log("No Rockets Left!");
            }
        }

        private void ShootAction() 
        {
            if (_shootStart)
                return;

            _shootStart = true;
            _duplicatorComponent.Activate();
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

        public void Upgrade(float Value = 0)
        {
            _rocketAmmo.UpgradeRocketMaxCount((int)(Value));
            _explosionRadiusModificatorUpgrade.ApplyModifier(Value);
        }

        public void Evolve()
        {
            _projectileFactory.Evolve(_data.EvolvedBulletData, () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<ExplosiveDamageAreaBullet>().SetExplosiveDamageAreaBullet((IReadableModificator)_explosionRadiusModificatorUpgrade));
        }

        public void Tick()
        {
            _projectileFactory.Tick();
            _duplicatorComponent?.Tick();
            _reloader.Update();
        }
    }
}
