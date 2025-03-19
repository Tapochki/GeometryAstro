using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TandC.GeometryAstro.Core
{
    public sealed class CoreScope : LifetimeScope
    {
        [SerializeField] GameConfig _gameConfig;
        [SerializeField] Player _player;
        [SerializeField] GameplayInputHandler _inputHandler;
        [SerializeField] GameplayCamera _gameplayCamera;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterInputHandler(builder);
            RegisterPlayer(builder);
            RegisterGameplayCamera(builder);
            RegisterEnemySystem(builder);
            RegisterWeaponController(builder);
            RegisterUIService(builder);
            RegisterTickService(builder);
            RegisterItemSpawner(builder);
            RegisterEntryPoint(builder);
        }

        private void RegisterUIService(IContainerBuilder builder)
        {
            builder.Register<UIService>(Lifetime.Scoped);
        }

        private void RegisterConfigs(IContainerBuilder builder) 
        {
            builder.RegisterInstance(_gameConfig).AsSelf();
        }

        private void RegisterInputHandler(IContainerBuilder builder) 
        {
            builder.RegisterComponent(_inputHandler).As<IGameplayInputHandler>();
        }

        private void RegisterPlayer(IContainerBuilder builder) 
        {
            builder.RegisterComponent(_player).AsSelf();
        }

        private void RegisterGameplayCamera(IContainerBuilder builder)
        {
            builder.RegisterComponent(_gameplayCamera).AsSelf();
        }

        private void RegisterItemSpawner(IContainerBuilder builder) 
        {
            Debug.LogError("RegisterEnemySystem");
            builder.Register<ItemSpawner>(Lifetime.Scoped).As<IItemSpawner>();
        }

        private void RegisterEnemySystem(IContainerBuilder builder)
        {
            Debug.LogError("RegisterEnemySystem");
            builder.Register<WaveController>(Lifetime.Scoped);
            builder.Register<EnemySpawner>(Lifetime.Scoped).As<IEnemySpawner>();
            builder.Register<EnemySpawnPositionService>(Lifetime.Scoped).As<IEnemySpawnPositionService>();
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CoreFlow>();
        }

        private void RegisterWeaponController(IContainerBuilder builder) 
        {
            builder.Register<WeaponController>(Lifetime.Scoped);
        }

        private void RegisterTickService(IContainerBuilder builder) 
        {
            builder.Register<TickService>(Lifetime.Scoped);
        }
    }
}