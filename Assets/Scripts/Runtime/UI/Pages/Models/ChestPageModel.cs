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
    public class ChestPageModel
    {
        public event Action LanguageChanged;

        private UIService _uiService;
        private LocalisationService _localisationService;
        private SoundService _soundService;
        private SkillService _skillService;

        private GameObject _selfObject;

        public List<SkillItem> CurrentSkillsList;
        public GameObject SelfObject
        {
            get
            {
                if (_selfObject == null)
                {
                    _selfObject = _uiService.Canvas.transform.Find("ChestPage").gameObject;
                }

                return _selfObject;
            }
        }

        public ChestPageModel(
            LocalisationService localisationService,
            SoundService soundService,
            UIService uiService,
            SkillService skillService)
        {
            _uiService = uiService;
            _localisationService = localisationService;
            _soundService = soundService;

            CurrentSkillsList = new List<SkillItem>();

            _localisationService.OnLanguageWasChangedEvent += OnLanguageWasChangedEventHandler;
            _skillService = skillService;
        }

        private void OnLanguageWasChangedEventHandler(Settings.Languages language)
        {
            LanguageChanged?.Invoke();
        }

        public string GetLocalisation(string key)
        {
            return _localisationService.GetString(key);
        }

        public void Confirm()
        {
            //_soundService.PlayClickSound();
            _uiService.OpenPage<GamePageView>();
        }

        public void Dispose()
        {
            _localisationService.OnLanguageWasChangedEvent -= OnLanguageWasChangedEventHandler;
        }

        public List<PreparationSkillData> GetSkills()
        {
            return _skillService.GetUpgradeOptions(true);
        }

        public void FillSkillList(SkillItem skill)
        {
            CurrentSkillsList.Add(skill);
        }

        public void ApplyAllSkils()
        {
            foreach (var item in CurrentSkillsList)
            {
                if (item.IsActive())
                {
                    item.SkillData.ApplySkill();
                }
            }
        }

        public string GetFormatedDescription(SkillUpgradeInfo info)
        {
            var newDescrtiption = "";
            newDescrtiption = GetLocalisation(info.Description);
            newDescrtiption = info.GetFormattedDescription(newDescrtiption);
            return newDescrtiption;
        }
    }
}