using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Manages the drone pool, spawn sequencing, and tick delegation.
    /// Attach to any GameObject (created at runtime via AddComponent).
    /// </summary>
    public class DroneModuleView : MonoBehaviour
    {
        private const int PreloadCount = 10;
        private const float TotalSpawnTime = 1f;
        private const float RotationSpeed = 360f;

        private BulletData _data;
        private float _damage;
        private float _critChance;
        private float _critMultiplier;

        private Transform _spawnPosition;
        private Action _onAllDronesReturned;

        private ObjectPool<Drone> _pool;
        private readonly List<ITickable> _activeDrones = new();

        private int _spawnCountRemaining;
        private float _spawnDelay;
        private float _spawnDelayTimer;
        private bool _isSpawning;
        private bool _isEvolved;

        // ── Init ──────────────────────────────────────────────────────────────────

        public void Init(
            BulletData data,
            float damage,
            float critChance,
            float critMultiplier,
            Transform spawnPosition,
            Action onAllDronesReturned)
        {
            _data = data;
            _damage = damage;
            _critChance = critChance;
            _critMultiplier = critMultiplier;
            _spawnPosition = spawnPosition;
            _onAllDronesReturned = onAllDronesReturned;
            _isEvolved = false;

            InitPool();
        }

        public void Evolve(BulletData evolvedData)
        {
            _isEvolved = true;
            _data = evolvedData;
            ClearPool();
            InitPool();
        }

        // ── Public API ────────────────────────────────────────────────────────────

        /// <summary>
        /// Begins spawning <paramref name="count"/> drones over ~1 second.
        /// Calls <c>onAllDronesReturned</c> when all have expired.
        /// </summary>
        public void StartDroneSpawn(int count)
        {
            _spawnDelay = TotalSpawnTime / Mathf.Max(count, 1);
            _spawnCountRemaining = count;
            SpawnNextDrone();
            _isSpawning = true;
        }

        /// <summary>Forces all active drones back to pool immediately.</summary>
        public void ForceStop()
        {
            _isSpawning = false;
            _spawnCountRemaining = 0;
            foreach (var d in _pool.GetAllActiveItems().ToArray())
                d.ForceReturn();
        }

        public void Tick()
        {
            transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);

            if (_isSpawning)
            {
                _spawnDelayTimer -= Time.deltaTime;
                if (_spawnDelayTimer <= 0f)
                    SpawnNextDrone();
            }

            for (int i = _activeDrones.Count - 1; i >= 0; i--)
                _activeDrones[i].Tick();
        }

        // ── Internal ──────────────────────────────────────────────────────────────

        private void InitPool()
        {
            _pool = new ObjectPool<Drone>(
                () => CreateDrone(),
                drone => drone.Show(),
                drone => DeactivateDrone(drone),
                PreloadCount);
        }

        private Drone CreateDrone()
        {
            Drone drone = Instantiate(_data.BulletObject, transform).GetComponent<Drone>();
            drone.Init(_data, _damage, _critChance, _critMultiplier, OnDroneReturned);
            drone.Hide();
            return drone;
        }

        private void DeactivateDrone(Drone drone)
        {
            _activeDrones.Remove(drone);
            drone.Hide();
        }

        private void SpawnNextDrone()
        {
            if (_spawnCountRemaining <= 0)
            {
                _isSpawning = false;
                return;
            }

            _spawnCountRemaining--;
            _spawnDelayTimer = _spawnDelay;

            if (_spawnCountRemaining <= 0)
                _isSpawning = false;

            Drone drone = _pool.Get();
            drone.SetEvolved(_isEvolved);
            drone.SetPosition(_spawnPosition.position);
            _activeDrones.Add(drone);
        }

        private void OnDroneReturned(Drone drone)
        {
            if (drone.IsOld)
            {
                _activeDrones.Remove(drone);
                drone.Dispose();
            }
            else
            {
                _pool.Return(drone);
            }

            if (_activeDrones.Count == 0)
                _onAllDronesReturned?.Invoke();
        }

        private void ClearPool()
        {
            foreach (var d in _pool.GetAllItemsInPool())
                if (d != null) d.Dispose();

            foreach (var d in _pool.GetAllActiveItems())
                d.SetOld();

            _pool.Dispose();
        }
    }
}
