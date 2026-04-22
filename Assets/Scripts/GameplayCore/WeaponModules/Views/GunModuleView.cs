using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Attach to the root of any gun weapon prefab (StandardGun, AutomaticGun, MachineGun, RifleGun).
    ///
    /// Prefab setup:
    ///   - Add one or more child GameObjects with WeaponShootingPattern components.
    ///   - GunModuleView auto-collects all WeaponShootingPattern children in Awake
    ///     (or you can assign them manually in the Inspector).
    ///
    /// Override OnReloadProgressChanged() in a subclass to drive a reload bar or VFX.
    /// </summary>
    public class GunModuleView : MonoBehaviour, IWeaponModuleView
    {
        [Tooltip("Leave empty to auto-collect all WeaponShootingPattern children on Awake.")]
        [SerializeField] private WeaponShootingPattern[] _patterns;

        private void Awake()
        {
            if (_patterns == null || _patterns.Length == 0)
                _patterns = GetComponentsInChildren<WeaponShootingPattern>();
        }

        public WeaponShootingPattern[] GetPatterns() => _patterns;

        /// <summary>
        /// Called every frame with reload progress (0 = reloading, 1 = ready).
        /// Override or extend to drive UI / animations.
        /// </summary>
        public virtual void OnReloadProgressChanged(float progress) { }
    }
}
