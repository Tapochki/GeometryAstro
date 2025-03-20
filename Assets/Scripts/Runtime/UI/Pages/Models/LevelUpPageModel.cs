using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.UI.Elements;
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
        private SkillService _skillService;

        private GameObject _selfObject;

        public List<SkillItem> CurrentSkillsList { get; private set; }
        private SkillItem _currentSelectedSkill;

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
            UIService uiService,
            SkillService skillService)
        {
            _uiService = uiService;
            _localisationService = localisationService;
            _soundService = soundService;
            _skillService = skillService;

            CurrentSkillsList = new List<SkillItem>();

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

        public void FillSkillList(SkillItem skill)
        {
            CurrentSkillsList.Add(skill);
        }

        public void SelecSkill(SkillItem skill)
        {
            _currentSelectedSkill = skill;
        }

        public List<PreparationSkillData> GetSkills()
        {
            _currentSelectedSkill = null;
            return _skillService.GetUpgradeOptions(false);
        }

        public string GetFormatedDescription(SkillUpgradeInfo info)
        {
            var newDescrtiption = "";
            newDescrtiption = GetLocalisation(info.Description);
            newDescrtiption = info.GetFormattedDescription(newDescrtiption);
            return newDescrtiption;
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
            _currentSelectedSkill.SkillData.ApplySkill();
        }

        public void Dispose()
        {
            _localisationService.OnLanguageWasChangedEvent -= OnLanguageWasChangedEventHandler;
        }
    }
}