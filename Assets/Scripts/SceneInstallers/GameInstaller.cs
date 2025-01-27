using TandC.EventBus;
using TandC.Gameplay;
using TandC.SceneSystems;
using UnityEngine;
using Zenject;

namespace TandC.SceneInstallers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Player _player;

        public override void InstallBindings()
        {
            InstallBusHolder();
            InstallPauseService();
            InstallInputHandler();
            InstallItemSpawner();
            InstallItemFactory();
            InstallWaveController();
            InstallEnemySpawnPositionService();
            InstallEnemySpawner();
            InstallEnemyFactory();
            InstallEnemyDeathProcessor();
            InstallLevelModel();
            InstallSkillService();

            InstallCameraBinding();
            InstallPlayerBinding();
        }

        private void InstallSkillService()
        {
            Container.Bind<ISkillService>().To<SkillService>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ISkillFactory>().To<SkillFactory>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallLevelModel() 
        {
            Container.Bind<LevelModel>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallItemSpawner() 
        {
            Container.Bind<IItemSpawner>().To<ItemSpawner>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallItemFactory() 
        {
            Container.Bind<IItemFactory>().To<ItemFactory>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallWaveController() 
        {
            Container.Bind<WaveController>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallEnemySpawnPositionService() 
        {
            Container.Bind<IEnemySpawnPositionService>().To<EnemySpawnPositionService>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallEnemySpawner() 
        {
            Container.Bind<IEnemySpawner>().To<EnemySpawner>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallEnemyFactory() 
        {
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallEnemyDeathProcessor() 
        {
            Container.Bind<IEnemyDeathProcessor>().To<EnemyDeathProcessor>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallInputHandler() 
        {
            Container.Bind<IGameplayInputHandler>().To<GameplayInputHandler>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallPauseService() 
        {
            Container.Bind<IPauseService>().To<PauseService>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallCameraBinding() 
        {
            Container.Bind<GameplayCamera>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallBusHolder()
        {
            Container.Bind<EventBusHolder>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallPlayerBinding()
        {
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
        }
    }
}