using TandC.GeometryAstro.Bootstrap.Units;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using TandC.GeometryAstro.Services;

namespace TandC.GeometryAstro.Bootstrap
{
    public class BootstrapFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneService _sceneManager;

        public BootstrapFlow(LoadingService loadingService, SceneService sceneManager)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
        }
        
        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();
            await _loadingService.BeginLoading(fooLoadingUnit);

            _sceneManager.LoadScene(RuntimeConstants.Scenes.Loading).Forget();
        }
    }
}
