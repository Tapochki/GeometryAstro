using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class MachineGun : IActiveSkill
    {
        public bool IsWeapon { get => true; }

        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private ActiveSkillData _data;

        private List<WeaponShootingPattern> _shootingPatterns = new();

        private int _shotsPerCycle = 20;
        private int _shotsFired;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.MachineGun;

        private DuplicatorComponent _duplicatorComponent;

        private bool _shootStart;

        private float _shootDeleyTimer;
        private float _shootDeleyTime = 0.1f;

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

        public void SetStartShotPerCycle(int value) 
        {
            _shotsPerCycle = value;
        }

        public void Initialization() 
        {
            _reloader.StartReload();
        }

        public void RegisterDuplicatorComponent(IReadableModificator duplicateModificator)
        {
            //_duplicatorComponent = new DuplicatorComponent(duplicateModificator, TryShoot, EndShoot);
        }

        public void RegisterShootingPatterns(Transform skillParent)
        {
            GameObject skillObject = MonoBehaviour.Instantiate(_data._skillPrefab, skillParent);
            _shootingPatterns = new List<WeaponShootingPattern>();
            foreach (var pattern in skillObject.GetComponentsInChildren<WeaponShootingPattern>())
            {
                if (pattern.Type == SkillType)
                {
                    _shootingPatterns.Add(pattern);
                }
            }
        }

        private void ShootAction() 
        {
            _shootStart = true;
            _shotsFired = 0; 
        }

        private void Shoot() 
        {
            foreach(var pattern in _shootingPatterns) 
            {
                TryShoot(pattern.Origin.position, pattern.Direction.position);
            }
        }

        private void TryShoot(Vector2 originPattern, Vector2 direction)
        {
            if (!_shootStart || _shotsFired >= _shotsPerCycle)
                return;

            Vector2 origin = originPattern;

            float spread = Random.Range(-_data.detectorRadius, _data.detectorRadius);

            Vector2 baseDirection = new Vector2(direction.x + spread, direction.y);

            CreateShoot(origin, baseDirection);

            _shotsFired++;
            if (_shotsFired >= _shotsPerCycle)
            {
                EndShoot();
            }
            _shootDeleyTimer = _shootDeleyTime;
        }

        private void EndShoot() 
        {
            _shootStart = false;
            _reloader.StartReload();
        }

        private void CreateShoot(Vector2 origin, Vector2 direction)
        {
            _projectileFactory.CreateProjectile(
                origin,
                direction
            );
        }

        public void Upgrade(float value = 0)
        {
            _shotsPerCycle += (int)value;
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
            if (_shootStart) 
            {
                _shootDeleyTimer -= Time.deltaTime;
                if(_shootDeleyTimer <= 0) 
                {
                    Shoot();
                }
            }
        }
    }
}
