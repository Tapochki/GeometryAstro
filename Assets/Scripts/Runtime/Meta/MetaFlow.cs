using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace TandC.GeometryAstro.Meta
{
    public class MetaFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneService _sceneService;

        public MetaFlow(LoadingService loadingService, SceneService sceneService)
        {
            _loadingService = loadingService;
            _sceneService = sceneService;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(new FooLoadingUnit(1));
            _sceneService.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}