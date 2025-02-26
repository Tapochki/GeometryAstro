using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Gameplay;
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
        [SerializeField] EnemySpawner _enemySpawner;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterInputHandler(builder);
            RegisterEventBus(builder);
            RegisterPlayer(builder);
            RegisterGameplayCamera(builder);
            EnemyWavesRegister(builder);
            RegisterEntryPoint(builder);
        }

        private void RegisterConfigs(IContainerBuilder builder) 
        {
            builder.RegisterInstance(_gameConfig).AsSelf();
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