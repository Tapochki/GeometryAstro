using System.Collections.Generic;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Standard auto-targeting gun module.
    /// Fires from up to 5 pattern points based on current level (1-5).
    /// DuplicatorComponent adds extra shots per trigger.
    ///
    /// Level 1 → pattern 0
    /// Level 2 → patterns 1, 2
    /// Level 3 → patterns 0, 1, 2
    /// Level 4 → patterns 1, 2, 3, 4
    /// Level 5 → all patterns
    /// </summary>
    public class StandardGunModule : IActiveSkill
    {
        public bool IsWeapon => true;

        private IProjectileFactory _factory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private DuplicatorComponent _duplicator;
        private WeaponShootingPattern[] _patterns;
        private ActiveSkillData _data;

        private int _currentLevel = 1;
        private bool _isShooting;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetProjectileFactory(IProjectileFactory factory) => _factory = factory;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;
        public void SetEnemyDetector(IEnemyDetector detector) => _enemyDetector = detector;
        public void SetPatterns(WeaponShootingPattern[] patterns) => _patterns = patterns;

        public void RegisterDuplicator(int extraDuplicates = 0)
            => _duplicator = new DuplicatorComponent(Shoot, EndShoot, extraDuplicates);

        // ── IActiveSkill ──────────────────────────────────────────────────────────

        public void Initialization() { }

        public void Upgrade(float value = 0)
        {
            if (_currentLevel < 5) _currentLevel++;
        }

        public void Evolve()
        {
            _duplicator?.UpgradeDuplicateCount();
            if (_data.EvolvedBulletData != null)
                _factory.Evolve(
                    _data.EvolvedBulletData,
                    () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<BaseBullet>());
        }

        public void Tick()
        {
            _reloader.Update();
            _factory.Tick();
            _duplicator?.Tick();

            if (_reloader.CanAction && !_isShooting)
                TryShoot();
        }

        // ── Internal ──────────────────────────────────────────────────────────────

        private void TryShoot()
        {
            if (_patterns == null || _patterns.Length == 0) return;

            WeaponShootingPattern detectionPattern = GetPatternById(0);
            if (detectionPattern == null) return;

            Transform enemy = _enemyDetector.GetEnemy(
                detectionPattern.Origin.position,
                detectionPattern.Direction.position,
                _data.DetectorRadius);

            if (enemy == null) return;

            _isShooting = true;
            _duplicator.Activate();
        }

        private void Shoot()
        {
            foreach (var pattern in GetActivePatterns())
                _factory.CreateProjectile(pattern.Origin.position, pattern.Direction.position);
        }

        private void EndShoot()
        {
            _isShooting = false;
            _reloader.StartReload();
        }

        private WeaponShootingPattern GetPatternById(int id)
        {
            if (_patterns == null) return null;
            foreach (var p in _patterns)
                if (p.Id == id) return p;
            return null;
        }

        private IEnumerable<WeaponShootingPattern> GetActivePatterns()
        {
            var result = new List<WeaponShootingPattern>();
            switch (_currentLevel)
            {
                case 1: AddById(0, result); break;
                case 2: AddById(1, result); AddById(2, result); break;
                case 3: AddById(0, result); AddById(1, result); AddById(2, result); break;
                case 4: AddById(1, result); AddById(2, result); AddById(3, result); AddById(4, result); break;
                default: result.AddRange(_patterns); break;
            }
            return result;
        }

        private void AddById(int id, List<WeaponShootingPattern> list)
        {
            var p = GetPatternById(id);
            if (p != null) list.Add(p);
        }
    }
}
