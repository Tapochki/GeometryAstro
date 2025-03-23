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
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private Player _player;
        [SerializeField] private GameplayInputHandler _inputHandler;
        [SerializeField] private GameplayCamera _gameplayCamera;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterPauseService(builder);
            RegisterInputHandler(builder);
            RegisterModificatorContainer(builder);
            RegisterPlayer(builder);
            RegisterGameplayCamera(builder);
            RegisterEnemySystem(builder);
            RegisterWeaponController(builder);
            RegisterUIService(builder);
            RegisterTickService(builder);
            RegisterItemSpawner(builder);
            RegisterSkillService(builder);
            RegisterEntryPoint(builder);
        }

        private void RegisterSkillService(IContainerBuilder builder)
        {
            builder.Register<SkillService>(Lifetime.Scoped);
        }

        private void RegisterPauseService(IContainerBuilder builder)
        {
            builder.Register<PauseService>(Lifetime.Scoped).As<IPauseService>();
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
            builder.Register<PlayerDeathProcessor>(Lifetime.Scoped);
        }

        private void RegisterGameplayCamera(IContainerBuilder builder)
        {
            builder.RegisterComponent(_gameplayCamera).AsSelf();
        }

        private void RegisterItemSpawner(IContainerBuilder builder)
        {
            builder.Register<ItemSpawner>(Lifetime.Scoped).As<IItemSpawner>();
        }

        private void RegisterModificatorContainer(IContainerBuilder builder)
        {
            builder.Register<ModificatorContainer>(Lifetime.Scoped);
        }

        private void RegisterEnemySystem(IContainerBuilder builder)
        {
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