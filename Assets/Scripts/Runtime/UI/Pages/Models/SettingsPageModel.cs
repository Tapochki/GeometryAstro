using System;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.UI
{
    public class SettingsPageModel
    {
        public event Action LanguageChanged;

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
                    _selfObject = _uiService.Canvas.transform.Find("SettingsPage").gameObject;
                }

                return _selfObject;
            }
        }

        public SettingsPageModel(
            LocalisationService localisationService,
            SoundService soundService,
            UIService uiService)
        {
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

        public void OpenMainMenu()
        {
            _uiService.OpenPage<MainMenuPageView>();
        }
    }
}