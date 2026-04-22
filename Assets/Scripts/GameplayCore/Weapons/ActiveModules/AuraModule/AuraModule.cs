using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Aura weapon module. Continuously rotates a damage zone around the player
    /// and deals damage to all enemies inside it on each reload tick.
    ///
    /// Setup:
    ///   1. <c>_data.SkillPrefab</c> — prefab that has an <c>AuraModuleView</c> component
    ///      with a Trigger Collider2D (the aura zone).
    ///   2. Assign via <c>AuraModuleSetup</c> on the Player GameObject.
    ///
    /// Upgrade: increases the aura collider size.
    /// Evolve:  switches to EvolvedBulletData (higher damage, changes color to red).
    /// </summary>
    public class AuraModule : IActiveSkill
    {
        public bool IsWeapon => true;

        private const float RotationSpeed = 45f;

        private ActiveSkillData _data;
        private IReloadable _reloader;

        private GameObject _skillObject;
        private AuraModuleView _view;

        private float _damage;
        private float _critChance;
        private float _critMultiplier;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void SetData(ActiveSkillData data) => _data = data;
        public void SetReloader(IReloadable reloader) => _reloader = reloader;

        /// <param name="skillParent">Transform to parent the aura prefab under.</param>
        /// <param name="damage">Base damage per tick.</param>
        /// <param name="critChance">Additive crit chance (0–1).</param>
        /// <param name="critMultiplier">Additive crit damage multiplier.</param>
        public void SetObject(Transform skillParent, float damage, float critChance, float critMultiplier)
        {
            _damage = damage;
            _critChance = critChance;
            _critMultiplier = critMultiplier;

            _skillObject = Object.Instantiate(_data.SkillPrefab, skillParent);
            _view = _skillObject.GetComponentInChildren<AuraModuleView>();
            _view.Init(_data.BulletData, _damage, _critChance, _critMultiplier);
        }

        // ── IActiveSkill ──────────────────────────────────────────────────────────

        public void Initialization() { }

        public void Upgrade(float value = 0)
        {
            _view.Upgrade(value);
        }

        public void Evolve()
        {
            if (_data.EvolvedBulletData != null)
                _view.Evolve(_data.EvolvedBulletData);
        }

        public void Tick()
        {
            _reloader.Update();

            _skillObject.transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);

            if (_reloader.CanAction)
            {
                _view.ApplyDamage();
                _reloader.StartReload();
            }
        }
    }
}
