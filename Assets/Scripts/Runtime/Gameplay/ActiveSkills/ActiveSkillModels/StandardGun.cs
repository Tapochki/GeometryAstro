using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class StandardGun : IActiveSkill
    {
        public bool IsWeapon { get => true; }

        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private IEnemyDetector _enemyDetector;
        private ActiveSkillData _data;

        private DuplicatorComponent _duplicatorComponent;

        private float _upgradeTimer = 10f;

        private List<WeaponShootingPattern> _shootingPatterns = new();

        private int _currentLevel = 1;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.StandartGun;

        private bool _shootStart;

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

        public void SetEnemyDetector(IEnemyDetector enemyDetector)
        {
            _enemyDetector = enemyDetector;
        }

        public void Initialization()
        {

        }

        public void RegisterDuplicatorComponent(IReadableModificator duplicateModificator) 
        {
            _duplicatorComponent = new DuplicatorComponent(duplicateModificator, Shoot, EndShoot);
        }

        public void RegisterShootingPatterns(Transform skillParent)
        {
            GameObject skillObject = MonoBehaviour.Instantiate(_data._skillPrefab, skillParent);

            foreach(var pattern in skillObject.GetComponentsInChildren<WeaponShootingPattern>())
            {
                if (pattern.Type == SkillType)
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
            Debug.LogError(_enemyDetector);
            Enemy enemy = _enemyDetector.GetEnemy(origin, direction, _data.detectorRadius);
            if(enemy == null) return;

            Vector2? enemyPosition = enemy.transform.position;

            if (enemyPosition.HasValue)
            {
                _shootStart = true;
                _duplicatorComponent.Activate();           
            }
        }

        private void EndShoot() 
        {
            _shootStart = false;
            _reloader.StartReload();
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

        public void Upgrade(float value = 0)
        {
            if (_currentLevel < 5) _currentLevel++;
        }

        public void Evolve()
        {
            _duplicatorComponent.UpgradeDuplicateCount();
            _projectileFactory.Evolve(_data.EvolvedBulletData, () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<StandartBullet>());
        }

        public void Tick()
        {
            _reloader.Update();
            _projectileFactory.Tick();
            _duplicatorComponent?.Tick();

            if (_reloader.CanAction && !_shootStart)
            {
                TryShoot();
            }
        }
    }
}


