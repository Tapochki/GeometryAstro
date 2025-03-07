namespace TandC.GeometryAstro.Gameplay 
{
    public interface IReloadable
    {
        float ReloadProgress { get; }
        void StartReload();
        public bool CanShoot { get; }
        public void Update();
    }
}

