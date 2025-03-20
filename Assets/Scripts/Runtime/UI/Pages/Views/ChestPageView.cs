using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.UI.Elements;
using TandC.GeometryAstro.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class ChestPageView : IUIPage, IEventReceiver<ChestItemReleaseEvent>
    {
        public bool IsActive { get; private set; }

        private ChestPageModel _model;

        private Transform _skillContainer;

        private GameObject _container;
        private GameObject _coinsObject;
        private GameObject _flashLight;

        private Animator _animator;

        private GameObject _template;

        public UniqueId Id { get; } = new UniqueId();

        public ChestPageView(ChestPageModel model)
        {
            _model = model;
            _model.LanguageChanged += LanguageChangedHandler;
            RegisterEvent();
        }

        public void Init()
        {
            GameObject selfObject = _model.SelfObject;
            Transform selfTransform = selfObject.transform;

            _animator = selfTransform.Find("Flashlight").GetComponent<Animator>();

            Button confirmButton = selfTransform.Find("Container/Button_Confirm").GetComponent<Button>();
            Button flashlightButton = selfTransform.Find("Flashlight").GetComponent<Button>();

            _skillContainer = selfTransform.Find("Container/Container_Skills");

            _container = selfTransform.Find("Container").gameObject;
            _coinsObject = selfTransform.Find("Container_Coins").gameObject;
            _flashLight = selfTransform.Find("Flashlight").gameObject;

            _template = _skillContainer.Find("Template").gameObject;
            _template.SetActive(false);

            confirmButton.onClick.AddListener(ConfirmButtonOnClick);
            flashlightButton.onClick.AddListener(FlashlightButtonOnClick);

            UpdateText();
        }

        private void LanguageChangedHandler()
        {
            UpdateText();
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this);
        }

        public void OnEvent(ChestItemReleaseEvent @event)
        {
            Show();
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

            TextMeshProUGUI confirmButtonTitle = selfObject.transform.
                Find("Container/Button_Confirm/Text_Main").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI openChestTitle = selfObject.transform.
                Find("Flashlight/Text_OpenChest").GetComponent<TextMeshProUGUI>();

            confirmButtonTitle.text = _model.GetLocalisation("KEY_CONFIRM");
            openChestTitle.text = _model.GetLocalisation("KEY_TAP_TO_OPEN_CHEST");
        }

        public void Show(object data = null)
        {
            _model.SelfObject.SetActive(true);

            _container.SetActive(false);
            _coinsObject.SetActive(false);
            _flashLight.SetActive(true);

            ShowSkillList();
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
        }

        private void ConfirmButtonOnClick()
        {
            _model.ApplyAllSkils();
            _model.Confirm();
            // TODO - play ClickSound
        }

        private void FlashlightButtonOnClick()
        {
            _animator.Play("ChestOpen", -1, 0);
            InternalTools.DoActionDelayed(() =>
            {

                _container.SetActive(true);
                _coinsObject.SetActive(true);
                _flashLight.SetActive(false);
            }, 1.0f);
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
                    true,
                    skillData,
                    new object[] { _model.GetLocalisation(info.Name), _model.GetFormatedDescription(info) }
                );

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