using System;
using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class StandardGun : MonoBehaviour, IWeapon, IDisposable
    {
        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private WeaponData _data;
        private CompositeDisposable _disposables = new();

        private List<WeaponShootingPattern> _shootingPatterns = new();

        [SerializeField]
        private Transform _bulletParent;
        [SerializeField]
        private WeaponConfig _config;
        private int _currentLevel = 1;

        private void Start() 
        {
            _data = _config.GetWeaponByType(WeaponType.StandardGun);
            _projectileFactory = new ProjectileFactory(_data.bulletData, _bulletParent, 50);
            _reloader = new WeaponReloader(_data.shootDeley);
            _enemyDetector = new RaycastEnemyDetector(LayerMask.GetMask("Enemy"));

            RegisterShootingPatterns();
        }

        private void RegisterShootingPatterns()
        {
            foreach (var pattern in FindObjectsOfType<WeaponShootingPattern>())
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
                Vector2 origin = pattern.Origin.position;
                Vector2 direction = pattern.Direction.position;

                _projectileFactory.CreateProjectile(
                    origin,
                    direction,
                    _data.baseDamage
                );
            }
        }

        private float _upgradeTimer = 10f;

        public void Update()
        {
            _reloader.Update();

            if (_reloader.CanShoot)
            {
                TryShoot();
            }

            _upgradeTimer -= Time.deltaTime;
            if (_upgradeTimer <= 0f)
            {
                Upgrade();
                Debug.LogError($"Upgrade: CurrentLevel {_currentLevel}");
                _upgradeTimer = 10f;
            }
        }

        public void Dispose() => _disposables.Dispose();

        public void UpdateWeapon(float deltaTime)
        {
           
        }

        public void Upgrade()
        {
            if (_currentLevel < 5) _currentLevel++;
        }
    }
}


