using System.Collections.Generic;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Rapid-fire burst gun. Fires <c>shotsPerCycle</c> bullets per reload,
    /// with a short delay between each shot.
    ///
    /// Evolve: halves shot delay, doubles shots per cycle, spreads in an arc.
    /// Upgrade: adds shots per cycle.
    /// </summary>
    public class MachineGun : IActiveSkill
    {
        public bool IsWeapon => true;

        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private ActiveSkillData _data;

        private List<WeaponShootingPattern> _patterns = new();

        private int _startShotsPerCycle;
        private int _shotsPerCycle;
        private int _shotsFired;
        private int _duplicateMultiplier = 1;

        private bool _isShooting;
        private float _shootDelayTimer;
        private float _shootDelayTime = 0.1f;
        private float _shotAngleStep;
        private bool _isEvolved;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetProjectileFactory(IProjectileFactory factory) => _projectileFactory = factory;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;

        /// <summary>How many shots fire per cycle before reload.</summary>
        public void SetStartShotsPerCycle(int value) => _startShotsPerCycle = value;

        public void RegisterShootingPatterns(Transform skillParent)
        {
            GameObject go = Object.Instantiate(_data.SkillPrefab, skillParent);
            foreach (var p in go.GetComponentsInChildren<WeaponShootingPattern>())
                _patterns.Add(p);
        }

        // ── IActiveSkill ──────────────────────────────────────────────────────────

        public void Initialization() => _reloader.StartReload();

        public void Upgrade(float value = 0)
        {
            _startShotsPerCycle += (int)value;
        }

        public void Evolve()
        {
            _shootDelayTime = 0.05f;
            _startShotsPerCycle *= 2;
            _isEvolved = true;
            if (_data.EvolvedBulletData != null)
            {
                _projectileFactory.Evolve(
                    _data.EvolvedBulletData,
                    () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<StandardBullet>());
            }
        }

        public void Tick()
        {
            _reloader.Update();
            _projectileFactory.Tick();

            if (_reloader.CanAction && !_isShooting)
                StartShooting();

            if (_isShooting)
            {
                _shootDelayTimer -= Time.deltaTime;
                if (_shootDelayTimer <= 0f)
                    Shoot();
            }
        }

        // ── Internal ──────────────────────────────────────────────────────────────

        private void StartShooting()
        {
            _shotsPerCycle = _startShotsPerCycle * _duplicateMultiplier;

            if (_isEvolved)
            {
                _shotAngleStep = (60f / _shotsPerCycle) * 2f;
                ResetPatternRotations();
            }

            _isShooting = true;
            _shotsFired = 0;
        }

        private void Shoot()
        {
            if (_isEvolved)
            {
                bool flipSide = true;
                foreach (var pattern in _patterns)
                {
                    ShootEvolved(pattern, flipSide);
                    flipSide = !flipSide;
                }
            }
            else
            {
                foreach (var pattern in _patterns)
                    ShootSpread(pattern);
            }
        }

        private void ShootEvolved(WeaponShootingPattern pattern, bool rightSide)
        {
            if (!CanShoot()) return;
            _projectileFactory.CreateProjectile(pattern.Origin.position, pattern.Direction.position);
            pattern.Origin.Rotate(0, 0, rightSide ? _shotAngleStep : -_shotAngleStep);
            ProcessShot();
        }

        private void ShootSpread(WeaponShootingPattern pattern)
        {
            if (!CanShoot()) return;
            float spread = Random.Range(-_data.SpreadRadius, _data.SpreadRadius);
            Vector2 dir = new(pattern.Direction.position.x + spread, pattern.Direction.position.y);
            _projectileFactory.CreateProjectile(pattern.Origin.position, dir);
            ProcessShot();
        }

        private bool CanShoot() => _isShooting && _shotsFired < _shotsPerCycle;

        private void ProcessShot()
        {
            _shotsFired++;
            _shootDelayTimer = _shootDelayTime;
            if (_shotsFired >= _shotsPerCycle)
                EndShooting();
        }

        private void EndShooting()
        {
            _isShooting = false;
            _reloader.StartReload();
        }

        private void ResetPatternRotations()
        {
            foreach (var p in _patterns)
                p.Origin.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
