using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace TandC.GeometryAstro.Loading
{
    public class LoadingFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneService _sceneService;

        public LoadingFlow(LoadingService loadingService, SceneService sceneService)
        {
            _loadingService = loadingService;
            _sceneService = sceneService;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(new FooLoadingUnit(1));
            _sceneService.LoadScene(RuntimeConstants.Scenes.Meta).Forget();
        }
    }
}