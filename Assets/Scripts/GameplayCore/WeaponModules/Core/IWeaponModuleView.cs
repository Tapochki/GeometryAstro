namespace GameplayCore
{
    /// <summary>
    /// Implemented by the MonoBehaviour on a gun weapon prefab's root.
    /// Provides shooting pattern transforms to the weapon model and
    /// receives reload state for optional UI feedback.
    /// </summary>
    public interface IWeaponModuleView
    {
        /// <summary>Returns all WeaponShootingPattern components on this prefab.</summary>
        WeaponShootingPattern[] GetPatterns();

        /// <summary>
        /// Called every model Tick with current reload progress.
        /// 0 = empty / reloading, 1 = ready to fire.
        /// Override to drive a reload bar or animation.
        /// </summary>
        void OnReloadProgressChanged(float progress);
    }
}
