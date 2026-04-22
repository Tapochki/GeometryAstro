using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Drone weapon module. Spawns a wave of orbiting drones that damage enemies
    /// on contact, then reloads for the next wave.
    ///
    /// The DroneModuleView is created at runtime parented to the player transform
    /// (so drones orbit the player ship).
    ///
    /// Upgrade: adds one extra drone per wave.
    /// Evolve: drones switch to EvolvedBulletData.
    /// </summary>
    public class DroneGunModule : IActiveSkill
    {
        public bool IsWeapon => true;

        private const int DefaultStartDroneCount = 2;

        private ActiveSkillData _data;
        private IReloadable _reloader;
        private DroneModuleView _view;

        private int _spawnCount = DefaultStartDroneCount;
        private bool _isActive;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;

        /// <summary>
        /// Assigns the DroneModuleView and initializes it with combat parameters.
        /// Call after the view is created/added via AddComponent.
        /// </summary>
        public void SetView(
            DroneModuleView view,
            BulletData bulletData,
            float damage,
            float critChance,
            float critMultiplier,
            Transform spawnPoint)
        {
            _view = view;
            _view.Init(bulletData, damage, critChance, critMultiplier, spawnPoint, OnWaveEnd);
        }

        /// <summary>Overrides the default starting drone count (2).</summary>
        public void SetStartSpawnCount(int count) => _spawnCount = count;

        // ── IActiveSkill ──────────────────────────────────────────────────────────

        public void Initialization() { }

        public void Upgrade(float value = 0)
            => _spawnCount++;

        public void Evolve()
        {
            if (_data.EvolvedBulletData != null)
                _view.Evolve(_data.EvolvedBulletData);
        }

        public void Tick()
        {
            _reloader.Update();
            _view.Tick();

            if (_reloader.CanAction && !_isActive)
            {
                _isActive = true;
                _view.StartDroneSpawn(_spawnCount);
            }
        }

        // ── Internal ──────────────────────────────────────────────────────────────

        private void OnWaveEnd()
        {
            _isActive = false;
            _reloader.StartReload();
        }
    }
}
