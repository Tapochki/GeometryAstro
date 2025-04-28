using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Gameplay.VFX;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.UI;
using TandC.GeometryAstro.Utilities;
using VContainer.Unity;

namespace TandC.GeometryAstro.Core
{
    public class CoreFlow : IStartable, IDisposable
    {
        private readonly GameConfig _gameConfig;

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

        private readonly IActiveSkillController _activeSkillControllerController;
        private readonly TickService _tickService;

        private readonly ModificatorContainer _modificatorContainer;

        private readonly LoadObjectsService _loadObjectsService;
        private readonly LocalisationService _localizationService;
        private readonly SoundService _soundService;
        private readonly UIService _uiService;
        private readonly SceneService _sceneService;

        private readonly SkillService _skillService;

        private readonly IPauseService _pauseService;

        private readonly ScoreContainer _scoreContainer;
        private readonly MoneyVaultContainer _moneyVaultContainer;

        private readonly VaultService _vaultService;
        private readonly IVFXService _vfxService;


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
            IActiveSkillController activeSkillController,
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
            MoneyVaultContainer moneyVaultContainer,
            VaultService vaultService,
            IVFXService vfxService,
            GameConfig gameConfig)
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
            _activeSkillControllerController = activeSkillController;
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
            _vaultService = vaultService;
            _vfxService = vfxService;
            _gameConfig = gameConfig;
        }

        public async void Start()
        {
            InitInputHandler();
            _modificatorContainer.Init();
            InitPlayer();
            InitItemSpawner();
            InitActiveSkillCointainer();
            InitEnemy();

            InitVaultCointainer();
            InitScoreContainer();

            RegisterEffect();

            RegisterUI();


            _skillService.Initialize();

            await LoadAssetsAsync();

            _pauseService.Init();

            OpenFirstPage();
        }

        private async Task LoadAssetsAsync()
        {
            var fooLoadingUnit = new FooLoadingUnit(0, false);
            await _loadingService.BeginLoading(_uiService);
            await _loadingService.BeginLoading(_vfxService);
            await _loadingService.BeginLoading(fooLoadingUnit);
        }

        private void InitInputHandler() 
        {
            _gameplayInputHandler.Init();
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
            var corePages = new List<IUIPage>
            {
                new GamePageView(new GamePageModel(_soundService, _uiService)),
                new SettingsPageView(new SettingsPageModel(_localizationService, _soundService, _uiService, _dataService)),
                new GameOverPageView(new GameOverPageModel(_sceneService, _localizationService, _soundService, _uiService, _playerDeathProcessor, _moneyVaultContainer, _scoreContainer, _vaultService)),
                new PausePageView(new PausePageModel(_sceneService, _localizationService, _soundService, this, _uiService)),
                new LevelUpPageView(new LevelUpPageModel(_localizationService, _soundService, _uiService, _skillService)),
                new ChestPageView(new ChestPageModel(_localizationService, _soundService, _uiService, _skillService)),
            };
            var corePopups = new List<IUIPopup>
            {

            };

            _uiService.RegisterUI(corePages, corePopups);
            UnityEngine.Debug.LogWarning("OPEN GAMEPLAY SCENE ON START");

        }

        private void OpenFirstPage() 
        {
            _uiService.OpenPage<GamePageView>();
        }

        private void RegisterEffect() 
        {
            List<IEffectContainer> effectContainers = new List<IEffectContainer>
            {
                new ExplosionEffectContainer(_gameConfig.EffectConfig),
                new DamageVFXContainer(_gameConfig.EffectConfig, _dataService),
            };

            _vfxService.RegisterEffectContainers(effectContainers);
        }

        private void InitItemSpawner()
        {
            _itemSpawner.Init();
        }

        private void InitActiveSkillCointainer()
        {
            _activeSkillControllerController.Init();
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