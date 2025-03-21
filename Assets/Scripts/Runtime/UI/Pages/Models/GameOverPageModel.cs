using System;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.UI
{
    public class GameOverPageModel
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
                    _selfObject = _uiService.Canvas.transform.Find("GameOverPage").gameObject;
                }

                return _selfObject;
            }
        }

        public GameOverPageModel(
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

        public void Continue()
        {
            //_soundService.PlayClickSound();
            //_sceneService.LoadScene(RuntimeConstants.Scenes.Core).Forget();
        }

        public void OneMoreChange()
        {
            //_soundService.PlayClickSound();
            //_uiService.OpenPage<>();
        }

        public void Dispose()
        {
            _localisationService.OnLanguageWasChangedEvent -= OnLanguageWasChangedEventHandler;
        }
    }
}