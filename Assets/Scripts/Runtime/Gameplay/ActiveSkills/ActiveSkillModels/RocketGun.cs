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

        public ActiveSkillType SkillType { get; private set; }

        private int _currentLevel = 1;

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

            reloadButton.Initialize(_reloader.ReloadProgress, _rocketAmmo.RocketCount, ShootAction);
        }

        private void InitRocketAmmo()
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
                if (pattern.Type == ActiveSkillType.RocketGun)
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

        public void Upgrade()
        {

        }

        public void Tick()
        {
            _duplicatorComponent?.Tick();
            _reloader.Update();
        }
    }
}
