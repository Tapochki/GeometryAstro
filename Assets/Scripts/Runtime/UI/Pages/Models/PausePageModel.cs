using System;
using TandC.GeometryAstro.Core;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.UI
{
    public class PausePageModel
    {
        public event Action LanguageChanged;

        private SceneService _sceneService;
        private UIService _uiService;
        private LocalisationService _localisationService;
        private SoundService _soundService;

        private CoreFlow _flow;

        private GameObject _selfObject;
        public GameObject SelfObject
        {
            get
            {
                if (_selfObject == null)
                {
                    _selfObject = _uiService.Canvas.transform.Find("PausePage").gameObject;
                }

                return _selfObject;
            }
        }

        public PausePageModel(
          SceneService sceneService,
          LocalisationService localisationService,
          SoundService soundService,
          CoreFlow flow,
          UIService uiService)
        {
            _sceneService = sceneService;
            _uiService = uiService;
            _localisationService = localisationService;
            _soundService = soundService;

            _flow = flow;

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

        public void LoadMenuScene()
        {
            //_soundService.PlayClickSound();

            _flow.LoadMenu();

            //_sceneService.LoadScene(RuntimeConstants.Scenes.Menu).Forget();
        }

        //public void OpenSettings()
        //{
        //    //_soundService.PlayClickSound();
        //    _uiService.OpenPage<SettingsPageView>();
        //}

        public void ContinueGame()
        {
            // TODO - unpause game
            //_soundService.PlayClickSound();
            _uiService.OpenPage<GamePageView>();
        }

        public void Dispose()
        {
            _localisationService.OnLanguageWasChangedEvent -= OnLanguageWasChangedEventHandler;
        }
    }
}