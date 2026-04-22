namespace GameplayCore
{
    /// <summary>
    /// Thin layer that binds a weapon model to its reloader.
    /// Exposes ReloadProgress for UI (reload bars, cooldown icons, etc.).
    ///
    /// Call Upgrade() / Evolve() on this from external systems (level-up, chest, etc.).
    /// </summary>
    public class WeaponModuleViewModel
    {
        /// <summary>The underlying weapon logic.</summary>
        public IActiveSkill Model { get; }

        /// <summary>The reloader shared between model and this ViewModel.</summary>
        public IReloadable Reloader { get; }

        /// <summary>0 = reloading, 1 = ready to fire.</summary>
        public float ReloadProgress => Reloader?.ReloadProgress ?? 1f;

        public WeaponModuleViewModel(IActiveSkill model, IReloadable reloader)
        {
            Model = model;
            Reloader = reloader;
        }
    }
}
