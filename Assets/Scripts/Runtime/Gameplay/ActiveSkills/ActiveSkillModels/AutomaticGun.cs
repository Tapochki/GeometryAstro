using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class AutomaticGun : IActiveSkill
    {
        public bool IsWeapon { get => true; }

        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private ActiveSkillData _data;

        private WeaponShootingPattern _weaponShootingPattern;

        private int _currentLevel = 1;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.AutoGun;

        private DuplicatorComponent _duplicatorComponent;

        private bool _shootStart;

        public void SetData(ActiveSkillData data) 
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
            _enemyDetector = enemyDetector;
        }

        public void Initialization() 
        {

        }

        public void RegisterDuplicatorComponent(IReadableModificator duplicateModificator)
        {
            _duplicatorComponent = new DuplicatorComponent(duplicateModificator, TryShoot, EndShoot);
        }

        public void RegisterShootingPatterns(Transform skillParent)
        {
            GameObject skillObject = MonoBehaviour.Instantiate(_data._skillPrefab, skillParent);

            foreach (var pattern in skillObject.GetComponentsInChildren<WeaponShootingPattern>())
            {
                if (pattern.Type == SkillType)
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
            Vector2? target = _enemyDetector.GetEnemyPosition(_weaponShootingPattern.Origin.position, default, _data.detectorRadius);
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

        public void Upgrade(float Value = 0)
        {
            _duplicatorComponent.UpgradeDuplicateCount();
        }

        public void Evolve()
        {
            _projectileFactory.Evolve(_data.EvolvedBulletData, () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<BulletWithHealth>());
        }

        public void Tick()
        {
            _duplicatorComponent?.Tick();
            _reloader.Update();
            _projectileFactory.Tick();
            if (_reloader.CanAction && !_shootStart)
            {
                ShootAction();
            }
        }
    }
}
