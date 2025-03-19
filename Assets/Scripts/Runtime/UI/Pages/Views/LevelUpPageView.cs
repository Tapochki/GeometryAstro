using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.UI.Elements;
using TandC.GeometryAstro.Utilities;
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
            Transform containerSkills = selfTransform.Find("Image_Background");
            Transform containerButtons = selfTransform.Find("Container_Buttons");

            _skillContainer = containerSkills.Find("Container_Skills");

            Button skillResetButton = containerButtons.Find("Button_ResetSkill").GetComponent<Button>();
            _confirmButton = containerButtons.Find("Button_Confirm").GetComponent<Button>();

            skillResetButton.onClick.AddListener(SkillReset);
            _confirmButton.onClick.AddListener(Confirm);

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
        }

        private void UpdateText()
        {
            //GameObject selfObject = _model.SelfObject;

            //TextMeshProUGUI startButtonTitle = selfObject.transform.
            //    Find("Container_Buttons/Button_Start/Text_Title").GetComponent<TextMeshProUGUI>();
            //TextMeshProUGUI shopButtonTitle = selfObject.transform.
            //    Find("Container_Buttons/Button_Shop/Text_Title").GetComponent<TextMeshProUGUI>();
            //TextMeshProUGUI leaderboardButtonTitle = selfObject.transform.
            //    Find("Container_Buttons/Button_Leaderboard/Text_Title").GetComponent<TextMeshProUGUI>();
            //TextMeshProUGUI infoDescriptionTitle = selfObject.transform.
            //    Find("Container_Advice/Container_Info/Text_Description").GetComponent<TextMeshProUGUI>();

            //startButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_START");
            //shopButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_SHOP");
            //leaderboardButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_LEADERBOARD");
            //infoDescriptionTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_VALUE_0");
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
            _confirmButton.interactable = false;

            _model.SelfObject.SetActive(true);
            ShowSkillList();
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

        private void ShowSkillList()
        {
            var template = _skillContainer.Find("Template").gameObject;
            var skillList = _model.GetSkills();
            InternalTools.ShuffleList(skillList);

            for (int i = 0; i < skillList.Count; i++)
            {
                var skillData = skillList[i];
                var info = skillData.SkillUpgradeInfo;

                SkillItem skillItem = new SkillItem(
                    template,
                    skillData.SkillUpgradeInfo.Level == 1,
                    false,
                    skillData,
                    new object[] { _model.GetLocalisation(info.Name), _model.GetFormatedDescription(info) }
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
                    foreach (var skill in _model._currentSkillsList)
                    {
                        skill.Deselect();
                    }
                };

                _model.FillSkillList(skillItem);
            }
        }
    }
}