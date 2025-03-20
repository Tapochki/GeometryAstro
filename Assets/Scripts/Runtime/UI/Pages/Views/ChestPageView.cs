using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class ChestPageView : IUIPage
    {
        public bool IsActive { get; private set; }

        private ChestPageModel _model;

        private Animator _animator;

        public ChestPageView(ChestPageModel model)
        {
            _model = model;
            _model.LanguageChanged += LanguageChangedHandler;
        }

        public void Init()
        {
            GameObject selfObject = _model.SelfObject;
            Transform selfTransform = selfObject.transform;

            _animator = selfTransform.Find("Flashlight").GetComponent<Animator>();

            Button confirmButton = selfTransform.Find("Container/Button_Confirm").GetComponent<Button>();

            confirmButton.onClick.AddListener(ConfirmButtonOnClick);

            UpdateText();
        }

        private void LanguageChangedHandler()
        {
            UpdateText();
        }

        public void Dispose()
        {
            _model.LanguageChanged -= LanguageChangedHandler;
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
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
        }

        private void ConfirmButtonOnClick()
        {
            _model.Confirm();
            // TODO - play ClickSound
        }
    }
}