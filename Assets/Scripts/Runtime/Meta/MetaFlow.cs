using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace TandC.GeometryAstro.Meta
{
    public class MetaFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneService _sceneManager;

        public MetaFlow(LoadingService loadingService, SceneService sceneManager)
        {
            _loadingService = loadingService;
            _sceneManager = sceneManager;
        }

        public async void Start()
        {
            await _loadingService.BeginLoading(new FooLoadingUnit(3));
            _sceneManager.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }
    }
}