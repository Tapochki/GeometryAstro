using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.UI;
using TandC.GeometryAstro.Utilities;
using VContainer.Unity;

namespace TandC.GeometryAstro.Menu
{
    public class MenuFlow : IStartable, IDisposable
    {
        private readonly LoadingService _loadingService;
        private readonly LoadObjectsService _loadObjectsService;
        private readonly DataService _dataService;
        private readonly LocalisationService _localizationService;
        private readonly SoundService _soundService;
        private readonly UIService _uiService;
        private readonly SceneService _sceneService;

        public MenuFlow(SceneService sceneService, LoadingService loadingService, LoadObjectsService loadObjectsService, DataService dataService, LocalisationService localizationService, SoundService soundService, UIService uiService)
        {
            _sceneService = sceneService;
            _loadingService = loadingService;
            _loadObjectsService = loadObjectsService;
            _dataService = dataService;
            _localizationService = localizationService;
            _soundService = soundService;
            _uiService = uiService;
        }

        public async void Start()
        {
            await LoadAssetsAsync();

            RegisterUI();
        }

        private async Task LoadAssetsAsync()
        {
            await _loadingService.BeginLoading(_uiService);
        }

        private void RegisterUI()
        {
            var mainMenuPages = new List<IUIPage>
            {
                new MainMenuPageView(new MainMenuPageModel(_sceneService, _localizationService, _soundService, _uiService)),
                new SettingsPageView(new SettingsPageModel(_localizationService, _soundService, _uiService, _dataService)),
            };
            var mainMenuPopups = new List<IUIPopup>
            {

            };

            _uiService.RegisterUI(mainMenuPages, mainMenuPopups); // register and initing
            _uiService.OpenPage<MainMenuPageView>();
        }

        public void Dispose()
        {
            _uiService.Dispose();
        }
    }
}