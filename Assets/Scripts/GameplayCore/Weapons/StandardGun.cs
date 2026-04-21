using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Standard auto-targeting gun. Detects enemies via IEnemyDetector, then fires
    /// projectiles from WeaponShootingPattern points.
    ///
    /// Level 1 → 1 barrel active
    /// Level 2 → 2 barrels
    /// Level 3 → 3 barrels
    /// Level 4 → 4 barrels
    /// Level 5 → all barrels
    ///
    /// Call Upgrade() to increase the level. Call Evolve() to add an extra duplicate shot.
    /// </summary>
    public class StandardGun : IActiveSkill
    {
        public bool IsWeapon => true;

        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private ActiveSkillData _data;

        private DuplicatorComponent _duplicator;
        private List<WeaponShootingPattern> _patterns = new();

        private int _currentLevel = 1;
        private bool _isShooting;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetProjectileFactory(IProjectileFactory factory) => _projectileFactory = factory;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;
        public void SetEnemyDetector(IEnemyDetector detector) => _enemyDetector = detector;

        /// <param name="extraDuplicates">Extra shots per trigger (0 = one shot, 1 = two shots…).</param>
        public void RegisterDuplicator(int extraDuplicates = 0)
        {
            _duplicator = new DuplicatorComponent(Shoot, EndShoot, extraDuplicates);
        }

        /// <summary>
        /// Instantiates the skill prefab under skillParent and collects all
        /// WeaponShootingPattern children, ordered by Id.
        /// </summary>
        public void RegisterShootingPatterns(Transform skillParent)
        {
            GameObject go = Object.Instantiate(_data.SkillPrefab, skillParent);
            _patterns = go.GetComponentsInChildren<WeaponShootingPattern>()
                          .OrderBy(p => p.Id)
                          .ToList();
        }

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
            {
                _projectileFactory.Evolve(
                    _data.EvolvedBulletData,
                    () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<BaseBullet>());
            }
        }

        public void Tick()
        {
            _reloader.Update();
            _projectileFactory.Tick();
            _duplicator?.Tick();

            if (_reloader.CanAction && !_isShooting)
                TryShoot();
        }

        // ── Internal ──────────────────────────────────────────────────────────────

        private void TryShoot()
        {
            // Use pattern 0 as the detection origin / aim reference
            WeaponShootingPattern detectionPattern = _patterns.FirstOrDefault(p => p.Id == 0);
            if (detectionPattern == null) return;

            Vector2 origin = detectionPattern.Origin.position;
            Vector2 aimDir = detectionPattern.Direction.position;

            Transform enemy = _enemyDetector.GetEnemy(origin, aimDir, _data.DetectorRadius);
            if (enemy == null) return;

            _isShooting = true;
            _duplicator.Activate();
        }

        private void Shoot()
        {
            foreach (WeaponShootingPattern pattern in GetActivePatterns())
            {
                _projectileFactory.CreateProjectile(
                    pattern.Origin.position,
                    pattern.Direction.position);
            }
        }

        private void EndShoot()
        {
            _isShooting = false;
            _reloader.StartReload();
        }

        private IEnumerable<WeaponShootingPattern> GetActivePatterns()
        {
            return _currentLevel switch
            {
                1 => GetById(0),
                2 => GetById(1, 2),
                3 => GetById(0, 1, 2),
                4 => GetById(1, 2, 3, 4),
                5 => _patterns,
                _ => GetById(0),
            };
        }

        private IEnumerable<WeaponShootingPattern> GetById(params int[] ids)
            => ids.Select(id => _patterns.FirstOrDefault(p => p.Id == id)).Where(p => p != null);
    }
}
