using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.UI;
using TandC.GeometryAstro.Utilities;
using UnityEngine;
using VContainer.Unity;

namespace TandC.GeometryAstro.Core
{
    public class CoreFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly DataService _dataService;
        private readonly Player _player;
        private readonly EventBusHolder _eventBusHolder;
        private readonly IGameplayInputHandler _gameplayInputHandler;
        private readonly GameplayCamera _gameplayCamera;
        private readonly LoadObjectsService _loadObjectsService;
        private readonly LocalisationService _localizationService;
        private readonly SoundService _soundService;
        private readonly UIService _uiService;
        private readonly SceneService _sceneService;

        private readonly WaveController _waveController;
        private readonly IEnemySpawner _enemySpawner;
        private readonly IEnemySpawnPositionService _enemySpawnPositionService;

        public CoreFlow(
            LoadingService loadingService,
            DataService dataService,
            Player player,
            EventBusHolder eventBusHolder,
            IGameplayInputHandler gameplayInputHandler,
            GameplayCamera gameplayCamera,
            WaveController waveController,
            IEnemySpawner enemySpawner,
            IEnemySpawnPositionService enemySpawnPositionService,
            LoadObjectsService loadObjectsService,
            LocalisationService localisationService,
            SoundService soundService,
            UIService uiService,
            SceneService sceneService)
        {
            _loadingService = loadingService;
            _dataService = dataService;
            _player = player;
            _eventBusHolder = eventBusHolder;
            _gameplayInputHandler = gameplayInputHandler;
            _gameplayCamera = gameplayCamera;
            _waveController = waveController;
            _enemySpawner = enemySpawner;
            _enemySpawnPositionService = enemySpawnPositionService;
            _loadObjectsService = loadObjectsService;
            _localizationService = localisationService;
            _soundService = soundService;
            _uiService = uiService;
            _sceneService = sceneService;
        }

        public async void Start()
        {
            _player.Init(_dataService.UserData.PlayerData);
            InitEnemy();

            var fooLoadingUnit = new FooLoadingUnit(3, false);



            await _loadingService.BeginLoading(fooLoadingUnit);
            await _loadingService.BeginLoading(_uiService);

            RegisterUI();
        }

        private void InitEnemy()
        {
            _enemySpawner.Init();
            _enemySpawnPositionService.Init();
            _waveController.Init();
        }

        public void LoadMenu()
        {
            _sceneService.LoadScene(RuntimeConstants.Scenes.Menu).Forget();
        }

        private void RegisterUI()
        {
            var corePages = new List<IUIPage>
            {
                new GamePageView(new GamePageModel(_soundService, _uiService)),
                new SettingsPageView(new SettingsPageModel(_localizationService, _soundService, _uiService, _dataService)),
                new PausePageView(new PausePageModel(_sceneService, _localizationService, _soundService, this, _uiService)),
            };
            var corePopups = new List<IUIPopup>
            {

            };

            _uiService.RegisterUI(corePages, corePopups); // register and initing
            UnityEngine.Debug.LogWarning("OPEN GAMEPLAY SCENE ON START");
            _uiService.OpenPage<GamePageView>();
        }
    }
}