using Cysharp.Threading.Tasks;
using System;
using TandC.GeometryAstro.Gameplay;
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

        private bool _isReviveAdViewed;
        private PlayerDeathProcessor _playerDeathProcessor;

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
            UIService uiService,
            PlayerDeathProcessor playerDeathProcessor)
        {
            _sceneService = sceneService;
            _uiService = uiService;
            _localisationService = localisationService;
            _soundService = soundService;

            _isReviveAdViewed = false;
            _playerDeathProcessor = playerDeathProcessor;

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
            _sceneService.LoadScene(RuntimeConstants.Scenes.Menu).Forget();
        }

        public bool IsOneMoreChanceButtonActive()
        {
            return _isReviveAdViewed;
        }

        public void OneMoreChance()
        {
            AdReviveViewedHandler(); // TODO change it to be called after watching an advertisement.

            //_soundService.PlayClickSound();
            //_uiService.OpenPage<>();
        }

        private void AdReviveViewedHandler()
        {
            _isReviveAdViewed = true;
            _playerDeathProcessor.StartReviveTimer();
        }

        public void Dispose()
        {
            _localisationService.OnLanguageWasChangedEvent -= OnLanguageWasChangedEventHandler;
        }
    }
}