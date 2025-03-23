using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.UI.Elements;
using TandC.GeometryAstro.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class LevelUpPageView : IUIPage, IEventReceiver<PlayerLevelUpEvent>
    {
        public bool IsActive { get; private set; }

        private LevelUpPageModel _model;

        private Transform _skillContainer;
        private Button _confirmButton;
        private Button _resetSkillsButton;

        public UniqueId Id { get; } = new UniqueId();

        public LevelUpPageView(LevelUpPageModel model)
        {
            _model = model;
            _model.LanguageChanged += LanguageChangedHandler;
            RegisterEvent();
        }

        public void Init()
        {
            GameObject selfObject = _model.SelfObject;
            Transform selfTransform = selfObject.transform;
            Transform container = selfTransform.Find("Image_Background");
            Transform containerButtons = container.Find("Container_Buttons");

            _skillContainer = container.Find("Container_Skills");

            _resetSkillsButton = containerButtons.Find("Button_ResetSkill").GetComponent<Button>();
            _confirmButton = containerButtons.Find("Button_Confirm").GetComponent<Button>();

            _resetSkillsButton.onClick.AddListener(SkillReset);
            _confirmButton.onClick.AddListener(Confirm);

            PrepareSkillList();

            UpdateText();
        }

        private void LanguageChangedHandler()
        {
            UpdateText();
        }

        public void Dispose()
        {
            _model.LanguageChanged -= LanguageChangedHandler;
            UnregisterEvent();
            _model.Dispose();
        }

        private void UpdateText()
        {
            GameObject selfObject = _model.SelfObject;

            TextMeshProUGUI titleTitle = selfObject.transform.
                Find("Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI skillResetButtonTitle = selfObject.transform.
                Find("Image_Background/Container_Buttons/Button_ResetSkill/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI confirmButtonTitle = selfObject.transform.
                Find("Image_Background/Container_Buttons/Button_Confirm/Text_Title").GetComponent<TextMeshProUGUI>();

            titleTitle.text = _model.GetLocalisation("KEY_LEVEL_UP_TITLE");
            skillResetButtonTitle.text = _model.GetLocalisation("KEY_RESET_SKILLS");
            confirmButtonTitle.text = _model.GetLocalisation("KEY_CONFIRM");
        }

        private void UpdateReachedLevelLocalisetion(int level)
        {
            GameObject selfObject = _model.SelfObject;

            TextMeshProUGUI reachedLevelTitle = selfObject.transform.
                Find("Image_Background/Text_Level").GetComponent<TextMeshProUGUI>();
            reachedLevelTitle.text = _model.GetLocalisation("KEY_REACHED_LEVEL") + ": " + level;
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this);
        }

        public void OnEvent(PlayerLevelUpEvent @event)
        {
            Show(@event.CurrentLevel);
        }

        public void Show(object data = null)
        {
            UpdateReachedLevelLocalisetion((int)data);
            _resetSkillsButton.interactable = true;
            _confirmButton.interactable = false;

            _model.SelfObject.SetActive(true);
            ShowSkillList();

            _model.PauseGame();
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
        }

        private void SkillReset()
        {
            _confirmButton.interactable = false;
            _resetSkillsButton.interactable = false;
            ShowSkillList();
            // TODO - play ClickSound
        }

        private void Confirm()
        {
            _model.Confirm();
            // TODO - play ClickSound
        }

        private void PrepareSkillList()
        {
            for (int i = 1; i <= 5; i++)
            {
                SkillItem skillItem = new SkillItem(
                    _skillContainer.Find($"Template_{i}").gameObject,
                    false
                );

                skillItem.ItemConfirmSelectionEvent += (PreparationSkillData) =>
                {
                    _model.Confirm();
                    // TODO unpause game
                };

                skillItem.ItemSelectionChangedEvent += (PreparationSkillData) =>
                {
                    _model.SelecSkill(skillItem);
                    _confirmButton.interactable = true;
                    DeselectAll();
                };

                _model.FillSkillList(skillItem);
            }
        }

        private void DeselectAll()
        {
            foreach (var skill in _model.CurrentSkillsList)
            {
                skill.Deselect();
            }
        }

        private void ShowSkillList()
        {
            DeselectAll();

            foreach (var item in _model.CurrentSkillsList)
            {
                item.Hide();
            }

            var skillList = _model.GetSkills();
            InternalTools.ShuffleList(skillList);
            for (int i = 0; i < skillList.Count; i++)
            {
                var skillData = skillList[i];
                var info = skillData.SkillUpgradeInfo;

                _model.CurrentSkillsList[i].UpdateData(
                    skillData.SkillUpgradeInfo.Level == 1,
                    skillData,
                    new object[] { _model.GetLocalisation(info.Name), _model.GetFormatedDescription(info) });
            }
        }
    }
}