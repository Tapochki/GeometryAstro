﻿using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.UI;
using TandC.GeometryAstro.Utilities;
using VContainer.Unity;

namespace TandC.GeometryAstro.Core
{
    public class CoreFlow : IStartable, IDisposable
    {
        private readonly LoadingService _loadingService;
        private readonly DataService _dataService;
        private readonly Player _player;
        private readonly PlayerDeathProcessor _playerDeathProcessor;
        private readonly IGameplayInputHandler _gameplayInputHandler;
        private readonly GameplayCamera _gameplayCamera;

        private readonly WaveController _waveController;
        private readonly IEnemySpawner _enemySpawner;
        private readonly IEnemySpawnPositionService _enemySpawnPositionService;

        private readonly IItemSpawner _itemSpawner;

        private readonly WeaponController _weaponController;
        private readonly TickService _tickService;

        private readonly ModificatorContainer _modificatorContainer;

        private readonly LoadObjectsService _loadObjectsService;
        private readonly LocalisationService _localizationService;
        private readonly SoundService _soundService;
        private readonly UIService _uiService;
        private readonly SceneService _sceneService;

        private readonly SkillService _skillService;

        private readonly IPauseService _pauseService;

        private ScoreContainer _scoreContainer;
        private MoneyVaultContainer _moneyVaultContainer;


        public CoreFlow(
            LoadingService loadingService,
            DataService dataService,
            Player player,
            PlayerDeathProcessor playerDeathProcessor,
            IGameplayInputHandler gameplayInputHandler,
            GameplayCamera gameplayCamera,
            WaveController waveController,
            IEnemySpawner enemySpawner,
            IEnemySpawnPositionService enemySpawnPositionService,
            TickService tickService,
            WeaponController weaponController,
            IItemSpawner itemSpawner,
            LoadObjectsService loadObjectsService,
            LocalisationService localizationService,
            SoundService soundService,
            UIService uiService,
            SceneService sceneService,
            SkillService skillService,
            ModificatorContainer modificatorContainer,
            IPauseService pauseService,
            ScoreContainer scoreContainer,
            MoneyVaultContainer moneyVaultContainer)
        {
            _loadingService = loadingService;
            _dataService = dataService;
            _player = player;
            _playerDeathProcessor = playerDeathProcessor;
            _gameplayInputHandler = gameplayInputHandler;
            _gameplayCamera = gameplayCamera;
            _waveController = waveController;
            _enemySpawner = enemySpawner;
            _enemySpawnPositionService = enemySpawnPositionService;
            _weaponController = weaponController;
            _tickService = tickService;
            _itemSpawner = itemSpawner;
            _loadObjectsService = loadObjectsService;
            _localizationService = localizationService;
            _soundService = soundService;
            _uiService = uiService;
            _sceneService = sceneService;
            _skillService = skillService;
            _modificatorContainer = modificatorContainer;
            _pauseService = pauseService;
            _scoreContainer = scoreContainer;
            _moneyVaultContainer = moneyVaultContainer;
        }

        public async void Start()
        {
            _modificatorContainer.Init();
            InitPlayer();
            InitItemSpawner();
            InitWeapon();
            InitEnemy();

            InitVaultCointainer();
            InitScoreContainer();

            var fooLoadingUnit = new FooLoadingUnit(0, false);
            _skillService.Initialize();
            await _loadingService.BeginLoading(_uiService);
            await _loadingService.BeginLoading(fooLoadingUnit);

            RegisterUI();

            _pauseService.Init();
        }

        private void InitVaultCointainer() 
        {
            _moneyVaultContainer.Init(_modificatorContainer.GetModificator(Settings.ModificatorType.ReceivingCoins));
        }

        private void InitScoreContainer() 
        {
            _scoreContainer.Init();
        }

        private void InitPlayer()
        {
            _playerDeathProcessor.Init(_modificatorContainer.GetModificator(Settings.ModificatorType.MaxHealth), _modificatorContainer.GetModificator(Settings.ModificatorType.ReviveCount));
            _player.Init(_dataService.UserData.PlayerData);
        }

        public void LoadMenu()
        {
            _sceneService.LoadScene(RuntimeConstants.Scenes.Menu).Forget();
        }

        private void RegisterUI()
        {
            // TODO -- СДЕЛАТЬ ЕБАНУЮ СТРУКТУРУ СКАЗАЛ ДАНИЛА
            var corePages = new List<IUIPage>
            {
                new GamePageView(new GamePageModel(_soundService, _uiService)),
                new SettingsPageView(new SettingsPageModel(_localizationService, _soundService, _uiService, _dataService)),
                new PausePageView(new PausePageModel(_sceneService, _localizationService, _soundService, this, _uiService)),
                new LevelUpPageView(new LevelUpPageModel(_localizationService, _soundService, _uiService, _skillService)),
                new ChestPageView(new ChestPageModel(_localizationService, _soundService, _uiService, _skillService)),
            };
            var corePopups = new List<IUIPopup>
            {

            };

            _uiService.RegisterUI(corePages, corePopups);
            UnityEngine.Debug.LogWarning("OPEN GAMEPLAY SCENE ON START");
            _uiService.OpenPage<GamePageView>();
        }

        private void InitItemSpawner()
        {
            _itemSpawner.Init();
        }

        private void InitWeapon()
        {
            _weaponController.Init();
            _weaponController.RegisterWeapon(Settings.WeaponType.StandardGun);
            //_weaponController.RegisterWeapon(Settings.WeaponType.RocketGun);
            //_weaponController.RegisterWeapon(Settings.WeaponType.AutoGun);
        }

        private void InitEnemy()
        {
            _enemySpawner.Init();
            _enemySpawnPositionService.Init();
            _waveController.Init();
        }

        public void Dispose()
        {
            _uiService.Dispose();
        }
    }
}