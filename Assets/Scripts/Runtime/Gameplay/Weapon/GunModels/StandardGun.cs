using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class StandardGun :IWeapon
    {
        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private WeaponData _data;

        private float _upgradeTimer = 10f;

        private List<WeaponShootingPattern> _shootingPatterns = new();

        private int _currentLevel = 1;

        public WeaponType WeaponType { get; private set; }

        public void SetData(WeaponData data)
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
            _enemyDetector = enemyDetector;
        }

        public void Initialization()
        {
            RegisterShootingPatterns();
        }

        private void RegisterShootingPatterns()
        {
            foreach (var pattern in GameObject.FindObjectsOfType<WeaponShootingPattern>())
            {
                if (pattern.Type == WeaponType.StandardGun)
                {
                    _shootingPatterns.Add(pattern);
                }
            }

            _shootingPatterns = _shootingPatterns.OrderBy(p => p.Id).ToList();
        }

        private IEnumerable<WeaponShootingPattern> GetActivePatterns()
        {
            List<WeaponShootingPattern> activePatterns = new();
            switch (_currentLevel)
            {
                case 1:
                    AddPatternIfExists(0, activePatterns);
                    break;
                case 2:
                    AddPatternIfExists(1, activePatterns);
                    AddPatternIfExists(2, activePatterns);
                    break;
                case 3:
                    AddPatternIfExists(0, activePatterns);
                    AddPatternIfExists(1, activePatterns);
                    AddPatternIfExists(2, activePatterns);
                    break;
                case 4:
                    AddPatternIfExists(1, activePatterns);
                    AddPatternIfExists(2, activePatterns);
                    AddPatternIfExists(3, activePatterns);
                    AddPatternIfExists(4, activePatterns);
                    break;
                case 5:
                    activePatterns.AddRange(_shootingPatterns);
                    break;
                default:
                    AddPatternIfExists(0, activePatterns);
                    break;
            }
            return activePatterns;
        }

        private void AddPatternIfExists(int id, List<WeaponShootingPattern> patterns)
        {
            var pattern = _shootingPatterns.FirstOrDefault(p => p.Id == id);
            if (pattern != null) patterns.Add(pattern);
        }

        private WeaponShootingPattern GetPatternById(int id)
        {
            return _shootingPatterns.FirstOrDefault(p => p.Id == id);
        }

        private void TryShoot()
        {
            WeaponShootingPattern detectionPattern = GetPatternById(0);

            if (detectionPattern == null) return;

            Vector2 origin = detectionPattern.Origin.position;
            Vector2 direction = detectionPattern.Direction.position;

            Vector2? enemyPosition = _enemyDetector.GetEnemyPosition(origin, direction, _data.detectorDistance);

            if (enemyPosition.HasValue)
            {
                Shoot();
                _reloader.StartReload();
            }
        }

        private void Shoot()
        {
            foreach (var pattern in GetActivePatterns())
            {
                Vector3 origin = pattern.Origin.transform.position;
                Vector3 direction = pattern.Direction.transform.position;

                _projectileFactory.CreateProjectile(
                    origin,
                    direction
                );
            }
        }

        public void UpdateWeapon(float deltaTime)
        {
           
        }

        public void Upgrade()
        {
            if (_currentLevel < 5) _currentLevel++;
        }

        public void Tick()
        {
            _reloader.Update();
            _projectileFactory.Tick();

            if (_reloader.CanShoot)
            {
                TryShoot();
            }

            _upgradeTimer -= Time.deltaTime;
            if (_upgradeTimer <= 0f)
            {
                Upgrade();
                _upgradeTimer = 10f;
            }
        }
    }
}


