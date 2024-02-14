using TandC.EventBus;
using TandC.Gameplay;
using TandC.SceneSystems;
using UnityEngine;
using Zenject;

namespace TandC.SceneInstallers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private Player _player;
        [SerializeField]
        private PlayerHealthView _playerHealthView;

        public override void InstallBindings()
        {
            InstallGameServiceBindings();
            InstallUiBindings();
            InstalPlayerBindings();
        }

        private void InstallUiBindings()
        {
            Container.Bind<PlayerHealthView>().FromInstance(_playerHealthView).AsSingle();
        }

        private void InstallGameServiceBindings() 
        {
            Container.Bind<PauseSystem>().FromComponentInHierarchy().AsSingle();
            Container.Bind<InputHandler>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ItemSpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ItemFactory>().FromComponentInHierarchy().AsSingle();
            Container.Bind<WaveController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EventBusHolder>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IEnemySpawnPositionService>().To<EnemySpawnPositionService>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IEnemySpawner>().To<EnemySpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IEnemyDeathProcessor>().To<EnemyDeathProcessor>().FromComponentInHierarchy().AsSingle();
        }

        private void InstalPlayerBindings()
        {
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
        }
    }
}