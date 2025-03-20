using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.UI;
using TandC.GeometryAstro.Utilities;
using VContainer.Unity;

namespace TandC.GeometryAstro.Core
{
    public class CoreFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly DataService _dataService;
        private readonly Player _player;
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

        public CoreFlow(
            LoadingService loadingService,
            DataService dataService,
            Player player,
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
            ModificatorContainer modificatorContainer)
        {
            _loadingService = loadingService;
            _dataService = dataService;
            _player = player;
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
        }

        public async void Start()
        {
            _modificatorContainer.Init();
            InitPlayer();
            InitItemSpawner();
            InitWeapon();
            InitEnemy();

            var fooLoadingUnit = new FooLoadingUnit(3, false);
            _skillService.Initialize();
            await _loadingService.BeginLoading(_uiService);
            await _loadingService.BeginLoading(fooLoadingUnit);

            RegisterUI();
        }

        private void InitPlayer()
        {
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
            _weaponController.RegisterWeapon(Settings.WeaponType.RocketGun);
            _weaponController.RegisterWeapon(Settings.WeaponType.AutoGun);
        }

        private void InitEnemy()
        {
            _enemySpawner.Init();
            _enemySpawnPositionService.Init();
            _waveController.Init();
        }
    }
}