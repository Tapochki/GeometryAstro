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

        private GameObject _template;

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

            _template = _skillContainer.Find("Template").gameObject;
            _template.SetActive(false);

            _resetSkillsButton = containerButtons.Find("Button_ResetSkill").GetComponent<Button>();
            _confirmButton = containerButtons.Find("Button_Confirm").GetComponent<Button>();

            _resetSkillsButton.onClick.AddListener(SkillReset);
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

            Time.timeScale = 0; // TODO costil
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
            Time.timeScale = 1; // TODO costil
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

        private void ShowSkillList()
        {
            ClearSkillsInView();

            var skillList = _model.GetSkills();
            InternalTools.ShuffleList(skillList);
            for (int i = 0; i < skillList.Count; i++)
            {
                var skillData = skillList[i];
                var info = skillData.SkillUpgradeInfo;

                SkillItem skillItem = new SkillItem(
                    MonoBehaviour.Instantiate(_template, _skillContainer),
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
                    Debug.Log(skillItem.SkillData.SkillType);
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

        private void ClearSkillsInView()
        {
            for (int i = 0; i < _skillContainer.childCount; i++)
            {
                GameObject child = _skillContainer.GetChild(i).gameObject;
                if (child.Equals(_template))
                {
                    continue;
                }

                MonoBehaviour.Destroy(child);
            }
        }
    }
}