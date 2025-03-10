using UniRx;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IReloadable
    {
        IReadOnlyReactiveProperty<float> ReloadProgress { get; }
        void StartReload();
        public bool CanShoot { get; }
        public void Update();
    }
}

