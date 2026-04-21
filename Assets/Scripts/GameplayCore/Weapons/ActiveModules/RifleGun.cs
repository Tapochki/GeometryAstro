using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Burst-sniper gun. Fires <c>startShotCount</c> spread shots per reload,
    /// each shot in quick succession (no delay between shots in cycle).
    ///
    /// Evolve: doubles shot count, fires in a 360° circle instead of spread.
    /// Upgrade: adds shots to the burst count.
    /// </summary>
    public class RifleGun : IActiveSkill
    {
        public bool IsWeapon => true;

        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private ActiveSkillData _data;

        private WeaponShootingPattern _pattern;

        private int _currentShots;
        private int _startShotCount;

        private float _circleRotationStep;
        private float _currentRotation;

        private bool _isShooting;
        private bool _isEvolved;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetProjectileFactory(IProjectileFactory factory) => _projectileFactory = factory;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;

        /// <summary>How many shots are fired per burst cycle before reload.</summary>
        public void SetStartShotsPerCycle(int value) => _startShotCount = value;

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

        public void Initialization() => _reloader.StartReload();

        public void Upgrade(float value = 0)
        {
            _startShotCount += (int)value;
        }

        public void Evolve()
        {
            _startShotCount *= 2;
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
                Shoot();
        }

        // ── Internal ──────────────────────────────────────────────────────────────

        private void StartShooting()
        {
            _currentShots = 0;
            _currentRotation = 0f;
            _isShooting = true;

            if (_isEvolved)
                _circleRotationStep = 360f / _startShotCount;
        }

        private void Shoot()
        {
            if (_isEvolved)
                ShootCircle();
            else
                ShootSpread();
        }

        private void ShootCircle()
        {
            if (_pattern == null) return;
            _pattern.Origin.rotation = Quaternion.Euler(0, 0, _currentRotation);
            _currentRotation += _circleRotationStep;
            _projectileFactory.CreateProjectile(_pattern.Origin.position, _pattern.Direction.position);
            ProcessShot();
        }

        private void ShootSpread()
        {
            if (_pattern == null) return;
            float spread = Random.Range(-_data.SpreadRadius, _data.SpreadRadius);
            Vector2 dir = new(_pattern.Direction.position.x + spread, _pattern.Direction.position.y);
            _projectileFactory.CreateProjectile(_pattern.Origin.position, dir);
            ProcessShot();
        }

        private void ProcessShot()
        {
            _currentShots++;
            if (_currentShots >= _startShotCount)
                EndShooting();
        }

        private void EndShooting()
        {
            _isShooting = false;
            _reloader.StartReload();
        }
    }
}
