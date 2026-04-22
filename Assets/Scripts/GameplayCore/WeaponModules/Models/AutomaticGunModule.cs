using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Auto-targeting gun module. Uses a CircleFirstEnemyDetector to find an enemy,
    /// then fires a burst of shots via DuplicatorComponent.
    ///
    /// Evolve: switches to BulletWithHealth projectiles.
    /// Upgrade: adds one extra duplicate shot per trigger.
    /// </summary>
    public class AutomaticGunModule : IActiveSkill
    {
        public bool IsWeapon => true;

        private IProjectileFactory _factory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private DuplicatorComponent _duplicator;
        private WeaponShootingPattern _pattern;
        private ActiveSkillData _data;

        private bool _isShooting;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetProjectileFactory(IProjectileFactory factory) => _factory = factory;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;
        public void SetEnemyDetector(IEnemyDetector detector) => _enemyDetector = detector;
        public void SetPattern(WeaponShootingPattern pattern) => _pattern = pattern;

        public void RegisterDuplicator(int extraDuplicates = 0)
            => _duplicator = new DuplicatorComponent(TryShootAtEnemy, EndShoot, extraDuplicates);

        // ── IActiveSkill ──────────────────────────────────────────────────────────

        public void Initialization() { }

        public void Upgrade(float value = 0)
            => _duplicator.UpgradeDuplicateCount();

        public void Evolve()
        {
            if (_data.EvolvedBulletData != null)
                _factory.Evolve(
                    _data.EvolvedBulletData,
                    () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<BulletWithHealth>());
        }

        public void Tick()
        {
            _duplicator.Tick();
            _reloader.Update();
            _factory.Tick();

            if (_reloader.CanAction && !_isShooting)
            {
                _isShooting = true;
                _duplicator.Activate();
            }
        }

        // ── Internal ──────────────────────────────────────────────────────────────

        private void TryShootAtEnemy()
        {
            if (_pattern == null) return;

            Transform enemy = _enemyDetector.GetEnemy(
                _pattern.Origin.position,
                default,
                _data.DetectorRadius);

            if (enemy != null)
                _factory.CreateProjectile(_pattern.Origin.position, enemy.position);
        }

        private void EndShoot()
        {
            _isShooting = false;
            _reloader.StartReload();
        }
    }
}
