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
        private readonly SceneService _sceneService;
        private readonly LoadObjectsService _loadObjectService;

        public BootstrapFlow(LoadingService loadingService, SceneService sceneService, DataService dataService, LoadObjectsService loadObjectService)
        {
            _loadingService = loadingService;
            _sceneService = sceneService;
            _dataService = dataService;
            _loadObjectService = loadObjectService;
        }
        
        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();
            await _loadingService.BeginLoading(fooLoadingUnit);
            await _loadingService.BeginLoading(_dataService);

            _sceneService.LoadScene(RuntimeConstants.Scenes.Loading).Forget();
        }
    }
}
