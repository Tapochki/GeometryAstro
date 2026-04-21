namespace GameplayCore
{
    public interface IReloadable
    {
        /// <summary>0 = empty, 1 = fully reloaded</summary>
        float ReloadProgress { get; }

        bool CanAction { get; }

        void StartReload();
        void Update();
    }
}
