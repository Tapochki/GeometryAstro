using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Aura weapon module. Continuously rotates a damage zone around the player
    /// and applies damage to all enemies inside it on each reload tick.
    ///
    /// Prefab setup (slot ViewPrefab):
    ///   - Root: any Transform (will be placed at weapon mount).
    ///   - Child: AuraModuleView component + Trigger Collider2D for the damage zone.
    ///
    /// Upgrade: increases aura collider size.
    /// Evolve: switches to EvolvedBulletData (higher damage, visual change).
    /// </summary>
    public class AuraGunModule : IActiveSkill
    {
        public bool IsWeapon => true;

        private const float RotationSpeed = 45f;

        private ActiveSkillData _data;
        private IReloadable _reloader;
        private GameObject _viewRootGO;
        private AuraModuleView _view;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;

        /// <param name="viewRootGO">The instantiated view prefab root (rotated every Tick).</param>
        /// <param name="view">The AuraModuleView component that applies damage.</param>
        public void SetView(GameObject viewRootGO, AuraModuleView view)
        {
            _viewRootGO = viewRootGO;
            _view = view;
        }

        // ── IActiveSkill ──────────────────────────────────────────────────────────

        public void Initialization() { }

        public void Upgrade(float value = 0)
            => _view.Upgrade(value);

        public void Evolve()
        {
            if (_data.EvolvedBulletData != null)
                _view.Evolve(_data.EvolvedBulletData);
        }

        public void Tick()
        {
            _reloader.Update();
            _viewRootGO.transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);

            if (_reloader.CanAction)
            {
                _view.ApplyDamage();
                _reloader.StartReload();
            }
        }
    }
}
