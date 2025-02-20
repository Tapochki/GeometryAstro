using TandC.GeometryAstro.Bootstrap.Units;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using TandC.GeometryAstro.Services;

namespace TandC.GeometryAstro.Bootstrap
{
    public class BootstrapFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly DataService _dataService;
        private readonly SceneService _sceneManager;

        public BootstrapFlow(LoadingService loadingService, SceneService sceneManager, DataService dataService)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
            _dataService = dataService;
        }
        
        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();
            await _loadingService.BeginLoading(fooLoadingUnit);
            await _loadingService.BeginLoading(_dataService);

            _sceneManager.LoadScene(RuntimeConstants.Scenes.Loading).Forget();
        }
    }
}
