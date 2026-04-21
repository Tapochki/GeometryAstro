using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Drone weapon module. Spawns a wave of orbiting drones that damage enemies
    /// on contact, then reloads for the next wave.
    ///
    /// Setup:
    ///   1. Assign <c>_data.SkillPrefab</c> — a prefab that has a child
    ///      WeaponShootingPattern (Origin = orbit center, Direction = spawn point).
    ///   2. <c>_data.BulletData.BulletObject</c> must be a prefab with a Drone component,
    ///      SpriteRenderer, and Trigger Collider2D.
    ///
    /// Upgrade: adds one extra drone per wave.
    /// Evolve:  drones become permanent (no lifetime), uses EvolvedBulletData.
    /// </summary>
    public class DroneModule : IActiveSkill
    {
        public bool IsWeapon => true;

        private const int StartDroneCount = 2;

        private ActiveSkillData _data;
        private IReloadable _reloader;
        private DroneModuleView _view;
        private WeaponShootingPattern _pattern;

        private int _spawnCount = StartDroneCount;
        private bool _isActive;
        private bool _isEvolved;

        // plain multiplier — increase via Upgrade or external code
        private int _spawnMultiplier = 1;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;

        /// <param name="skillParent">Transform where the skill prefab is spawned.</param>
        /// <param name="viewParent">Transform the DroneModuleView rotates around (usually the player).</param>
        /// <param name="damage">Base damage per drone hit.</param>
        /// <param name="critChance">Additive crit chance.</param>
        /// <param name="critMultiplier">Additive crit damage multiplier.</param>
        public void SetObject(Transform skillParent, Transform viewParent,
            float damage, float critChance, float critMultiplier)
        {
            GameObject go = Object.Instantiate(_data.SkillPrefab, skillParent);
            foreach (var p in go.GetComponentsInChildren<WeaponShootingPattern>())
            {
                _pattern = p;
                break;
            }

            var viewGO = new GameObject("DroneModuleView");
            viewGO.transform.SetParent(viewParent);
            viewGO.transform.localPosition = Vector3.zero;
            _view = viewGO.AddComponent<DroneModuleView>();
            _view.Init(_data.BulletData, damage, critChance, critMultiplier,
                _pattern.Direction, OnWaveEnd);
        }

        // ── IActiveSkill ──────────────────────────────────────────────────────────

        public void Initialization() { }

        public void Upgrade(float value = 0)
        {
            _spawnCount++;
        }

        public void Evolve()
        {
            _isEvolved = true;
            if (_data.EvolvedBulletData != null)
                _view.Evolve(_data.EvolvedBulletData);
        }

        public void Tick()
        {
            _reloader.Update();
            _view.Tick();

            if (_reloader.CanAction && !_isActive)
                LaunchWave();
        }

        // ── Internal ──────────────────────────────────────────────────────────────

        private void LaunchWave()
        {
            _isActive = true;
            _view.StartDroneSpawn(_spawnCount * _spawnMultiplier);
        }

        private void OnWaveEnd()
        {
            _isActive = false;
            _reloader.StartReload();
        }
    }
}
