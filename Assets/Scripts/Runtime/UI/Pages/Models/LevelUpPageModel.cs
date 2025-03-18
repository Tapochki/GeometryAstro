using System;
using System.Collections;
using System.Collections.Generic;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.UI
{
    public class LevelUpPageModel
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
                    _selfObject = _uiService.Canvas.transform.Find("LevelUpPage").gameObject;
                }

                return _selfObject;
            }
        }

        public LevelUpPageModel(
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

        public void SkillReset()
        {
            //_soundService.PlayClickSound();
            //_uiService.OpenPage<>();
        }

        public void Confirm()
        {
            //_soundService.PlayClickSound();
            _uiService.OpenPage<GamePageView>();
        }
    }
}