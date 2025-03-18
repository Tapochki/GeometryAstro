using System;
using System.Collections;
using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class LevelUpPageView : IUIPage
    {
        public bool IsActive { get; private set; }

        private LevelUpPageModel _model;

        public LevelUpPageView(LevelUpPageModel model)
        {
            _model = model;
            _model.LanguageChanged += LanguageChangedHandler;
        }

        public void Init()
        {
            GameObject selfObject = _model.SelfObject;
            Transform selfTransform = selfObject.transform;
            Transform containerButtons = selfTransform.Find("Container_Buttons");

            Button skillResetButton = containerButtons.Find("Button_Start").GetComponent<Button>();
            Button confirmButton = containerButtons.Find("Button_Shop").GetComponent<Button>();

            skillResetButton.onClick.AddListener(SkillReset);
            confirmButton.onClick.AddListener(Confirm);

            UpdateText();
        }

        private void LanguageChangedHandler()
        {
            UpdateText();
        }

        public void Dispose()
        {
            _model.LanguageChanged -= LanguageChangedHandler;
        }

        private void UpdateText()
        {
            GameObject selfObject = _model.SelfObject;

            TextMeshProUGUI startButtonTitle = selfObject.transform.
                Find("Container_Buttons/Button_Start/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI shopButtonTitle = selfObject.transform.
                Find("Container_Buttons/Button_Shop/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI leaderboardButtonTitle = selfObject.transform.
                Find("Container_Buttons/Button_Leaderboard/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI infoDescriptionTitle = selfObject.transform.
                Find("Container_Advice/Container_Info/Text_Description").GetComponent<TextMeshProUGUI>();

            startButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_START");
            shopButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_SHOP");
            leaderboardButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_LEADERBOARD");
            infoDescriptionTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_VALUE_0");
        }

        public void Show(object data = null)
        {
            _model.SelfObject.SetActive(true);
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
        }

        private void SkillReset()
        {
            _model.SkillReset();
            // TODO - play ClickSound
        }

        private void Confirm()
        {
            _model.Confirm();
            // TODO - play ClickSound
        }
    }

    public class SkillItem
    {
        public event Action<SkillType> ItemSelectionChangedEvent;
        public event Action<SkillType> ItemConfirmSelectionEvent;

        public GameObject selfObject;

        private Button _selectButton;
        private Button _buttonConfirmSelection;

        public bool isSelect { get; private set; }

        public bool isChestSkill;

        public SkillType skillType;
        public uint skillId;

        public SkillItem(GameObject prefab, bool isNew, bool isChestSkill = false)
        {
            // HARDCORE START

            var description = "HADCORE TEST DESCRIPTION";
            var isPercent = UnityEngine.Random.Range(0, 1) == 1;
            var percentIncrese = UnityEngine.Random.Range(0.0f, 20.0f);
            var value = UnityEngine.Random.Range(0, 100);
            var skillName = "TEST SKILL NAME";
            uint skillID = 1234567890;
            SkillType type = SkillType.StandartGun;

            // HARDCORE END

            selfObject = prefab;
            _selectButton = selfObject.GetComponent<Button>();
            _buttonConfirmSelection = _selectButton.transform.Find("Button_Confirm").GetComponent<Button>();
            _buttonConfirmSelection.gameObject.SetActive(false);

            // HARDCORE need to setup sprite from scriptable object
            //selfObject.transform.Find("Image_IconBackground/Image_Icon").GetComponent<Image>().sprite = data.sprite;

            string text = description;
            if (isPercent)
            {
                text = text.Replace("%n%", $"<color=#FFBF00>{percentIncrese}%</color>");
            }
            else
            {
                text = text.Replace("%n%", $"<color=#FFBF00>{value}</color>");
            }
            selfObject.transform.Find("Text_SkillDescription").GetComponent<TextMeshProUGUI>().text = text;
            selfObject.transform.Find("Text_SkillName").GetComponent<TextMeshProUGUI>().text = skillName;
            selfObject.transform.Find("Text_NewMark").gameObject.SetActive(isNew);
            skillId = skillID;
            skillType = type;
            SetSelection(false);
            this.isChestSkill = isChestSkill;
            _selectButton.onClick.AddListener(SelectButtonOnClickHandler);
            _buttonConfirmSelection.onClick.AddListener(ConfirmSelectionButtonOnClickHandler);

            if (isChestSkill)
            {
                _selectButton.interactable = false;
            }
        }

        private void SelectButtonOnClickHandler()
        {
            SetSelection(!isSelect);
        }

        public void SetSelection(bool state)
        {
            if (isSelect == state)
            {
                return;
            }

            ItemSelectionChangedEvent?.Invoke(skillType);
            isSelect = state;
            selfObject.GetComponent<Image>().color = new Color(0.0f, 0.4f, 0.6f, 1f);
            _buttonConfirmSelection.gameObject.SetActive(true);
        }

        public void Deselect()
        {
            selfObject.GetComponent<Image>().color = new Color(0.0f, 0.32f, 0.32f, 1f);
            isSelect = false;
            _buttonConfirmSelection.gameObject.SetActive(false);
        }

        private void ConfirmSelectionButtonOnClickHandler()
        {
            ItemConfirmSelectionEvent?.Invoke(skillType);
        }

        public void Dispose()
        {
            MonoBehaviour.Destroy(selfObject);
        }
    }
}