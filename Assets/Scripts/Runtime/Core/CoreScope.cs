using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Utilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TandC.GeometryAstro.Core
{
    public sealed class CoreScope : LifetimeScope
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private Player _player;
        [SerializeField] private GameplayInputHandler _inputHandler;
        [SerializeField] private GameplayCamera _gameplayCamera;
        [SerializeField] private EnemySpawner _enemySpawner;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterInputHandler(builder);
            RegisterEventBus(builder);
            RegisterPlayer(builder);
            RegisterGameplayCamera(builder);
            EnemyWavesRegister(builder);

            RegisterUIService(builder);

            RegisterEntryPoint(builder);
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gameConfig).AsSelf();
        }

        private void RegisterUIService(IContainerBuilder builder)
        {
            builder.Register<UIService>(Lifetime.Scoped);
        }

        private void RegisterInputHandler(IContainerBuilder builder)
        {
            builder.RegisterComponent(_inputHandler).As<IGameplayInputHandler>();
        }

        private void RegisterEventBus(IContainerBuilder builder)
        {
            builder.Register<EventBusHolder>(Lifetime.Scoped);
        }

        private void RegisterPlayer(IContainerBuilder builder)
        {
            builder.RegisterComponent(_player).AsSelf();
        }

        private void RegisterGameplayCamera(IContainerBuilder builder)
        {
            builder.RegisterComponent(_gameplayCamera).AsSelf();
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CoreFlow>();
        }

        private void EnemyWavesRegister(IContainerBuilder builder)
        {
            builder.Register<WaveController>(Lifetime.Scoped);
            builder.Register<EnemySpawner>(Lifetime.Scoped).As<IEnemySpawner>();
            builder.Register<EnemySpawnPositionService>(Lifetime.Scoped).As<IEnemySpawnPositionService>();
        }
    }
}