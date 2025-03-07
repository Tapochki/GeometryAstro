using Cysharp.Threading.Tasks;
using TandC.GeometryAstro.Bootstrap.Units;
using TandC.GeometryAstro.Services;
using VContainer.Unity;

namespace TandC.GeometryAstro.Bootstrap
{
    public class BootstrapFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly DataService _dataService;
        private readonly SceneService _sceneService;
        private readonly LoadObjectsService _loadObjectService;
        private readonly LocalisationService _localisationService;
        private readonly SoundService _soundService;

        public BootstrapFlow(
            LoadingService loadingService,
            SceneService sceneService,
            DataService dataService,
            LoadObjectsService loadObjectService,
            SoundService soundService,
            LocalisationService localisationService)
        {
            _loadingService = loadingService;
            _sceneService = sceneService;
            _soundService = soundService;
            _dataService = dataService;
            _loadObjectService = loadObjectService;
            _localisationService = localisationService;
        }

        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();
            await _loadingService.BeginLoading(fooLoadingUnit);
            await _loadingService.BeginLoading(_localisationService);
            await _loadingService.BeginLoading(_dataService);

            _sceneService.LoadScene(RuntimeConstants.Scenes.Loading).Forget();
        }
    }
}
