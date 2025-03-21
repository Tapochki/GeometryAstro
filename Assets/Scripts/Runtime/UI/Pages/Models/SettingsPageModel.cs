using System;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Settings;
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
        private DataService _dataService;

        private Languages[] languageList = (Languages[])Enum.GetValues(typeof(Languages));
        private int currentLanguageIndex;

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
            UIService uiService,
            DataService dataService)
        {
            _uiService = uiService;
            _localisationService = localisationService;
            _soundService = soundService;

            _localisationService.OnLanguageWasChangedEvent += OnLanguageWasChangedEventHandler;
            _dataService = dataService;
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

        public void ChangeMusicVolume(float value)
        {

        }

        public void ChangeSoundVolume(float value)
        {

        }

        public void NextLocalisation()
        {
            currentLanguageIndex = (currentLanguageIndex + 1) % languageList.Length;
            _localisationService.UpdateLocalisation(languageList[currentLanguageIndex]);
        }

        public void PreviousLocalisation()
        {
            currentLanguageIndex = (currentLanguageIndex - 1 + languageList.Length) % languageList.Length;
            _localisationService.UpdateLocalisation(languageList[currentLanguageIndex]);
        }

        public void Dispose()
        {
            _localisationService.OnLanguageWasChangedEvent -= OnLanguageWasChangedEventHandler;
        }
    }
}