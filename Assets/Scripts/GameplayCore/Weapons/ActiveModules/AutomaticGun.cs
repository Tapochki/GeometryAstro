using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Auto-targeting gun. Detects enemies with a circle cast, then fires
    /// a burst of shots using the DuplicatorComponent for multi-shot upgrades.
    ///
    /// Evolve: switches to BulletWithHealth projectiles.
    /// Upgrade: adds one extra duplicate shot per trigger.
    /// </summary>
    public class AutomaticGun : IActiveSkill
    {
        public bool IsWeapon => true;

        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private ActiveSkillData _data;

        private WeaponShootingPattern _pattern;
        private DuplicatorComponent _duplicator;

        private bool _isShooting;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetProjectileFactory(IProjectileFactory factory) => _projectileFactory = factory;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;
        public void SetEnemyDetector(IEnemyDetector detector) => _enemyDetector = detector;

        public void RegisterShootingPatterns(Transform skillParent)
        {
            GameObject go = Object.Instantiate(_data.SkillPrefab, skillParent);
            foreach (var p in go.GetComponentsInChildren<WeaponShootingPattern>())
            {
                _pattern = p;
                break;
            }
        }

        // ── IActiveSkill ──────────────────────────────────────────────────────────

        public void Initialization()
        {
            _duplicator = new DuplicatorComponent(TryShootAtEnemy, EndShoot, extraDuplicates: 0);
        }

        public void Upgrade(float value = 0)
        {
            _duplicator.UpgradeDuplicateCount();
        }

        public void Evolve()
        {
            if (_data.EvolvedBulletData != null)
            {
                _projectileFactory.Evolve(
                    _data.EvolvedBulletData,
                    () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<BulletWithHealth>());
            }
        }

        public void Tick()
        {
            _duplicator.Tick();
            _reloader.Update();
            _projectileFactory.Tick();

            if (_reloader.CanAction && !_isShooting)
                StartShootAction();
        }

        // ── Internal ──────────────────────────────────────────────────────────────

        private void StartShootAction()
        {
            _isShooting = true;
            _duplicator.Activate();
        }

        private void TryShootAtEnemy()
        {
            if (_pattern == null) return;
            Transform enemy = _enemyDetector.GetEnemy(_pattern.Origin.position, default, _data.DetectorRadius);
            if (enemy != null)
                _projectileFactory.CreateProjectile(_pattern.Origin.position, enemy.position);
        }

        private void EndShoot()
        {
            _isShooting = false;
            _reloader.StartReload();
        }
    }
}
