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
        private readonly VaultService _vaultService;

        public BootstrapFlow(
            LoadingService loadingService,
            SceneService sceneService,
            DataService dataService,
            LoadObjectsService loadObjectService,
            SoundService soundService,
            LocalisationService localisationService,
            VaultService vaultService)
        {
            _loadingService = loadingService;
            _sceneService = sceneService;
            _soundService = soundService;
            _dataService = dataService;
            _loadObjectService = loadObjectService;
            _localisationService = localisationService;
            _vaultService = vaultService;
        }

        public async void Start()
        {
            var fooLoadingUnit = new FooLoadingUnit();

            await _loadingService.BeginLoading(fooLoadingUnit);
            await _loadingService.BeginLoading(_localisationService);
            await _loadingService.BeginLoading(_dataService);
            await _loadingService.BeginLoading(_vaultService);

            _sceneService.LoadScene(RuntimeConstants.Scenes.Loading).Forget();
        }
    }
}
