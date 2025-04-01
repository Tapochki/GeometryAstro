using Newtonsoft.Json.Linq;
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

        private int _startShotsPerCycle;
        private int _shotsPerCycle;
        private int _shotsFired;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.MachineGun;

        private IReadableModificator _duplicatorModificator;

        private bool _isShooting;

        private float _shootDelayTimer;
        private const float _shootDelayTime = 0.1f;
        private float _shotAngleStep;

        private bool _isEvolved;

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetProjectileFactory(IProjectileFactory projectileFactory) => _projectileFactory = projectileFactory;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;
        public void SetStartShotsPerCycle(int value) => _startShotsPerCycle = value;

        public void Initialization() => _reloader.StartReload(); 

        public void RegisterDuplicatorComponent(IReadableModificator duplicateModificator)
        {
            _duplicatorModificator = duplicateModificator;
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

        private void StartShooting()
        {
            Debug.LogError((int)_duplicatorModificator.Value);
            _shotsPerCycle = _startShotsPerCycle * (int)_duplicatorModificator.Value;
            if (_isEvolved)
            {
                _shotAngleStep = (120f / _shotsPerCycle) * 2;
                ResetPatternRotation();
            }

            _isShooting = true;
            _shotsFired = 0;
        }

        private void Shoot()
        {
            if (_isEvolved)
            {
                bool isRightOrLeft = true;
                foreach (var pattern in _shootingPatterns)
                {
                    TryShootEvolved(pattern, isRightOrLeft);
                    isRightOrLeft = !isRightOrLeft;
                }
            }
            else
            {
                foreach (var pattern in _shootingPatterns)
                    TryShoot(pattern);
            }
        }

        private void TryShootEvolved(WeaponShootingPattern pattern, bool isRightOrLeft)
        {
            if (!CanShoot()) return;

            CreateProjectile(pattern.Origin.position, pattern.Direction.position);
            AdjustPatternRotation(pattern, isRightOrLeft);
            ProcessShot();
        }

        private void TryShoot(WeaponShootingPattern pattern)
        {
            if (!CanShoot()) return;

            float spread = Random.Range(-_data.detectorRadius, _data.detectorRadius);
            Vector2 direction = new(pattern.Direction.position.x + spread, pattern.Direction.position.y);

            CreateProjectile(pattern.Origin.position, direction);
            ProcessShot();
        }

        private void CreateProjectile(Vector2 origin, Vector2 direction)
        {
            _projectileFactory.CreateProjectile(origin, direction);
        }

        private bool CanShoot() => _isShooting && _shotsFired < _shotsPerCycle;

        private void ProcessShot()
        {
            _shotsFired++;
            _shootDelayTimer = _shootDelayTime;
            if (_shotsFired >= _shotsPerCycle) EndShooting();
        }

        private void EndShooting()
        {
            _isShooting = false;
            _reloader.StartReload();
        }

        private void ResetPatternRotation()
        {
            foreach (var pattern in _shootingPatterns)
                pattern.Origin.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void AdjustPatternRotation(WeaponShootingPattern pattern, bool isRightOrLeft)
        {
            pattern.Origin.Rotate(0, 0, isRightOrLeft ? _shotAngleStep : -_shotAngleStep);
        }

        public void Upgrade(float value = 0) => _startShotsPerCycle += (int)value;

        public void Evolve()
        {
            _startShotsPerCycle *= 2;
            _isEvolved = true;
            _projectileFactory.Evolve(_data.EvolvedBulletData, () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<StandartBullet>());
        }

        public void Tick()
        {
            _reloader.Update();
            _projectileFactory.Tick();

            if (_reloader.CanAction && !_isShooting) StartShooting();

            if (_isShooting)
            {
                _shootDelayTimer -= Time.deltaTime;
                if (_shootDelayTimer <= 0) Shoot();
            }
        }
    }
}
