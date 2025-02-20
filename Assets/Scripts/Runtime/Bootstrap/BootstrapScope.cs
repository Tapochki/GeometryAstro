using TandC.GeometryAstro.Services;
using VContainer;
using VContainer.Unity;

namespace TandC.GeometryAstro.Bootstrap
{
    public sealed class BootstrapScope : LifetimeScope
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(this);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LoadingService>(Lifetime.Singleton);
            builder.Register<DataService>(Lifetime.Singleton).AsSelf();
            builder.Register<SceneService>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}