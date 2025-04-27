using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class DroneSkillView : MonoBehaviour
    {
        private const int _preloadDroneCount = 10;
        private const float _totalSpawnTime = 1f;

        private IReadableModificator _damageModificator;
        private IReadableModificator _criticalChanceModificator;
        private IReadableModificator _criticalDamageMultiplier;

        private float _spawnDeleyTimer;
        private float _spawnDeleyTime;

        private BulletData _data;

        private ObjectPool<Drone> _pool;

        private int _spawnCount;
        private bool _isEvolve;

        private List<ITickable> _activeDrones;
        private Transform _spawnDronePosition;

        private Action _droneEndAction;

        private bool _startSpawn;

        private float _rotationSpeed = 360f;

        public void Init(BulletData data,
            IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplier,
            Transform spawnTransform,
            Action droneEndAction
            )
        {
            _isEvolve = false;
            _data = data;
            _damageModificator = damageModificator;
            _criticalChanceModificator = criticalChanceModificator;
            _criticalDamageMultiplier = criticalDamageMultiplier;
            _spawnDronePosition = spawnTransform;
            _droneEndAction = droneEndAction;
            _activeDrones = new List<ITickable>();

            InitializePool();
        }

        private void InitializePool()
        {
            _pool = new ObjectPool<Drone>(
                preloadFunc: () => CreateDrone(),
                getAction: drone => drone.Show(),
                returnAction: drone => DeactivateDrone(drone),
                _preloadDroneCount);
        }

        private void DeactivateDrone(Drone drone)
        {
            _activeDrones.Remove(drone);
            drone.Hide();
        }

        private void DroneEnd(Drone drone) 
        {
            if (drone.IsOld)
            {
                DeleteOldActiveDrone(drone);
                return;
            }

            _pool.Return(drone);
            if(_activeDrones.Count == 0) 
            {
                _droneEndAction?.Invoke();
            }
        }

        private void DeleteOldActiveDrone(Drone drone)
        {
            _activeDrones.Remove(drone);
            drone.Dispose();

            if (_activeDrones.Count == 0)
            {
                _droneEndAction?.Invoke();
            }
        }

        private void ClearOldBullets()
        {
            var oldDronesInPool = _pool.GetAllItemInPool();
            var activeOldDrones = _pool.GetAllActiveItem();

            DestroyOldDrons(oldDronesInPool, activeOldDrones);

            _pool.Dispose();
        }

        private void DestroyOldDrons(List<Drone> oldDronesInPool, List<Drone> activeOldDrones)
        {
            foreach (Drone bullet in oldDronesInPool)
            {
                if (bullet != null)
                    bullet.Dispose();
            }

            foreach (Drone bullet in activeOldDrones)
            {
                bullet.SetOldDroneOld();
            }

            oldDronesInPool.Clear();
        }

        private Drone CreateDrone() 
        {
            Drone drone = MonoBehaviour.Instantiate(_data.BulletObject, gameObject.transform).GetComponent<Drone>();
            drone.Init(_data, _damageModificator, _criticalChanceModificator, _criticalDamageMultiplier, DroneEnd);
            drone.Hide();
            return drone;
        }

        public void Evolve(BulletData evolvedData)
        {
            _data = evolvedData;
            _isEvolve = true;

            ClearOldBullets();

            InitializePool();
        }

        private void SetDrone()
        {
            _spawnCount--;
            _spawnDeleyTimer = _spawnDeleyTime;
            if (_spawnCount <= 0 ) 
            {
                _startSpawn = false;
            }
            Drone drone = _pool.Get();
            drone.SetEvolve(_isEvolve);
            drone.SetPosition(_spawnDronePosition.position);
            _activeDrones.Add(drone);
        }

        public void ForseStop() 
        {
            var activeDrones = _pool.GetAllActiveItem();
            _startSpawn = false;
            _spawnCount = 0;
            foreach (var drone in activeDrones) 
            {
                drone.ForcedFadeOut();
            }
        }

        public void StartDroneSpawn(int spawnCount) 
        {
            _spawnDeleyTime = _totalSpawnTime / spawnCount;
            _spawnCount = spawnCount;
            SetDrone();
            _startSpawn = true;
        }

        public void Tick() 
        {
            gameObject.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
            if (_startSpawn) 
            {
                _spawnDeleyTimer -= Time.deltaTime;
                if (_spawnDeleyTimer < 0)
                {
                    SetDrone();
                }
            }

            for (int i = _activeDrones.Count - 1; i >= 0; i--)
            {
                ITickable bullet = _activeDrones[i];
                bullet.Tick();
            }
        }
    }
}

