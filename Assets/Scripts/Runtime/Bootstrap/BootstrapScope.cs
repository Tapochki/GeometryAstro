using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TandC.GeometryAstro.Bootstrap
{
    public sealed class BootstrapScope : LifetimeScope
    {
        [SerializeField] private MenuConfig _menuConfig;

        protected override void Awake()
        {
            DontDestroyOnLoad(this);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            builder.Register<LoadingService>(Lifetime.Singleton);
            builder.Register<LocalisationService>(Lifetime.Singleton);
            builder.Register<SoundService>(Lifetime.Singleton);
            builder.Register<DataService>(Lifetime.Singleton).AsSelf();
            builder.Register<SceneService>(Lifetime.Singleton);
            builder.Register<LoadObjectsService>(Lifetime.Singleton);
            builder.Register<VaultService>(Lifetime.Singleton);

            builder.RegisterEntryPoint<BootstrapFlow>();
        }


        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_menuConfig).AsSelf();
        }
    }
}