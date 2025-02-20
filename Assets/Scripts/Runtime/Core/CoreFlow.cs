using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.Services;
using VContainer.Unity;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.EventBus;

namespace TandC.GeometryAstro.Core
{
    public class CoreFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly DataService _dataService;
        private readonly Player _player;
        private readonly EventBusHolder _eventBusHolder;
        private readonly IGameplayInputHandler _gameplayInputHandler;
        private GameplayCamera _gameplayCamera;

        public CoreFlow(LoadingService loadingService, DataService dataService, Player player, EventBusHolder eventBusHolder, IGameplayInputHandler gameplayInputHandler, GameplayCamera gameplayCamera)
        {
            _loadingService = loadingService;
            _dataService = dataService;
            _player = player;
            _eventBusHolder = eventBusHolder;
            _gameplayInputHandler = gameplayInputHandler;
            _gameplayCamera = gameplayCamera;
        }

        public async void Start()
        {
            _player.Init(_dataService.UserData.PlayerData);

            var fooLoadingUnit = new FooLoadingUnit(3, false);
            await _loadingService.BeginLoading(fooLoadingUnit);
        }
    }
}