using System;
using TandC.Data;
using TandC.EventBus;
using TandC.Gameplay;
using TandC.SceneSystems;
using TandC.Settings;
using UnityEngine;
using Zenject;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace TandC.SceneInstallers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private Player _player;
        [SerializeField]
        private PlayerHealthView _playerHealthView;
        [SerializeField]
        private GameObject _enemyPrefab;
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
            Container.Bind<EnemySpawnPositionService>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EnemyFactory>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EnemyDeathProcessor>().FromComponentInHierarchy().AsSingle();
        }

        private void InstalPlayerBindings()
        {
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
        }
    }
}