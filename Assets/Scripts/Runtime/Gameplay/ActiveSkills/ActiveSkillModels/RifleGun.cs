using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class RifleGun : IActiveSkill
    {
        public bool IsWeapon { get => true; }

        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private ActiveSkillData _data;

        private WeaponShootingPattern _shootingPattern;

        private int _currentShots;
        private int _startShootCount;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.RifleGun;

        private IReadableModificator _duplicatorModificator;

        private bool _isShooting;

        private bool _isEvolved;

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetProjectileFactory(IProjectileFactory projectileFactory) => _projectileFactory = projectileFactory;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;
        public void SetStartShotsPerCycle(int value) => _startShootCount = value;

        public void Initialization() => _reloader.StartReload(); 

        public void RegisterDuplicatorComponent(IReadableModificator duplicateModificator)
        {
            _duplicatorModificator = duplicateModificator;
        }

        public void RegisterShootingPatterns(Transform skillParent)
        {
            GameObject skillObject = MonoBehaviour.Instantiate(_data._skillPrefab, skillParent);
            foreach (var pattern in skillObject.GetComponentsInChildren<WeaponShootingPattern>())
            {
                if (pattern.Type == SkillType)
                {
                    _shootingPattern = pattern;
                    break;
                }
            }
        }

        private void StartShooting()
        {
            _currentShots = 0;
            _isShooting = true;
        }

        private void Shoot()
        {
            if (_isEvolved)
            {

            }
            else
            {
                TryShoot();
            }
        }

        private void TryShoot()
        {
            float spread = Random.Range(-_data.detectorRadius, _data.detectorRadius);
            Vector2 direction = new(_shootingPattern.Direction.position.x + spread, _shootingPattern.Direction.position.y);

            CreateProjectile(_shootingPattern.Origin.position, direction);
            _currentShots++;

            if(_currentShots >= _startShootCount)
                EndShooting();
        }

        private void CreateProjectile(Vector2 origin, Vector2 direction)
        {
            Debug.LogError(1);
            _projectileFactory.CreateProjectile(origin, direction);
        }

        private void EndShooting()
        {
            _isShooting = false;
            _reloader.StartReload();
        }

        public void Upgrade(float value = 0) => _startShootCount += (int)value;

        public void Evolve()
        {
            _startShootCount *= 2;
            _isEvolved = true;
            _projectileFactory.Evolve(_data.EvolvedBulletData, () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<StandartBullet>());
        }

        public void Tick()
        {
            _reloader.Update();
            _projectileFactory.Tick();

            if (_reloader.CanAction && !_isShooting) 
                StartShooting();

            if (_isShooting)
            {
                Shoot();
            }
        }
    }
}
