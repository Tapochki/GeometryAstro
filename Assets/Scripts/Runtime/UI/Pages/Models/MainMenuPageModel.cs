using Cysharp.Threading.Tasks;
using System;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.UI
{
    public class MainMenuPageModel
    {
        public event Action LanguageChanged;

        private SceneService _sceneService;
        private UIService _uiService;
        private LocalisationService _localisationService;
        private SoundService _soundService;

        private GameObject _selfObject;
        public GameObject SelfObject
        {
            get
            {
                if (_selfObject == null)
                {
                    _selfObject = _uiService.Canvas.transform.Find("MenuPage").gameObject;
                }

                return _selfObject;
            }
        }

        public MainMenuPageModel(
            SceneService sceneService,
            LocalisationService localisationService,
            SoundService soundService,
            UIService uiService)
        {
            _sceneService = sceneService;
            _uiService = uiService;
            _localisationService = localisationService;
            _soundService = soundService;

            _localisationService.OnLanguageWasChangedEvent += OnLanguageWasChangedEventHandler;
        }

        private void OnLanguageWasChangedEventHandler(Settings.Languages language)
        {
            LanguageChanged?.Invoke();
        }

        public string GetLocalisation(string key)
        {
            return _localisationService.GetString(key);
        }

        public void LoadGameScene()
        {
            //_soundService.PlayClickSound();
            _sceneService.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }

        public void OpenShop()
        {
            //_soundService.PlayClickSound();
            //_uiService.OpenPage<>();
        }

        public void OpenLeaderstats()
        {
            //_soundService.PlayClickSound();
            //_uiService.OpenPage<>();
        }

        public void OpenSetting()
        {
            //_soundService.PlayClickSound();
            _uiService.OpenPage<SettingsPageView>();
        }

        public void Dispose()
        {

        }
    }
}