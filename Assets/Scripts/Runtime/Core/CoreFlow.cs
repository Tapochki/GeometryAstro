using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.Services;
using VContainer.Unity;
using TandC.GeometryAstro.Gameplay;

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

        private WeaponController _weaponController;
        private TickService _tickService;

        public CoreFlow(LoadingService loadingService, DataService dataService, Player player, IGameplayInputHandler gameplayInputHandler, GameplayCamera gameplayCamera,
            WaveController waveController, IEnemySpawner enemySpawner, IEnemySpawnPositionService enemySpawnPositionService, TickService tickService, WeaponController weaponController, IItemSpawner itemSpawner)
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
        }

        public async void Start()
        {
            InitPlayer();
            InitItemSpawner();
            InitWeapon();
            InitEnemy();

            var fooLoadingUnit = new FooLoadingUnit(3, false);
            await _loadingService.BeginLoading(fooLoadingUnit);
        }

        private void InitPlayer()
        {
            _player.Init(_dataService.UserData.PlayerData);
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