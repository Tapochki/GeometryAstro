using System.Collections.Generic;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Drop on the Player GameObject alongside Player.cs.
    /// Reads PlayerWeaponConfig, spawns all weapon views and registers each with Player.
    ///
    /// Setup:
    ///   1. Assign a PlayerWeaponConfig asset.
    ///   2. Assign WeaponMount — a child Transform on the ship where weapon prefabs are parented.
    ///      If left empty the Player root transform is used.
    ///   3. Fill the config's Slots with weapon types, view prefabs and data.
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class ActiveModuleController : MonoBehaviour
    {
        [SerializeField] private PlayerWeaponConfig _config;

        [Tooltip("Parent Transform on the ship for weapon prefabs. Defaults to Player root.")]
        [SerializeField] private Transform _weaponMount;

        private readonly List<WeaponModuleViewModel> _modules = new();

        private void Awake()
        {
            if (_config == null)
            {
                Debug.LogError("[ActiveModuleController] PlayerWeaponConfig not assigned!", this);
                return;
            }

            Player player = GetComponent<Player>();
            Transform mount = _weaponMount != null ? _weaponMount : transform;

            foreach (var slot in _config.Slots)
            {
                if (slot == null || slot.ModuleType == WeaponModuleType.None)
                    continue;

                WeaponModuleViewModel vm = ActiveModuleFactory.Create(slot, mount, transform);
                if (vm == null)
                    continue;

                _modules.Add(vm);
                player.RegisterWeapon(vm.Model);
            }
        }

        // ── Public API ────────────────────────────────────────────────────────────

        /// <summary>Returns the ViewModel for the given slot index (0-based).</summary>
        public WeaponModuleViewModel GetModule(int slotIndex)
            => slotIndex >= 0 && slotIndex < _modules.Count ? _modules[slotIndex] : null;

        /// <summary>Returns all active modules.</summary>
        public IReadOnlyList<WeaponModuleViewModel> GetAllModules() => _modules;

        /// <summary>Upgrades the module in the given slot.</summary>
        public void Upgrade(int slotIndex, float value = 0)
            => GetModule(slotIndex)?.Model.Upgrade(value);

        /// <summary>Evolves the module in the given slot.</summary>
        public void Evolve(int slotIndex)
            => GetModule(slotIndex)?.Model.Evolve();
    }
}
