using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class SettingsPageView : IUIPage
    {
        public bool IsActive { get; private set; }

        private SettingsPageModel _model;

        public SettingsPageView(SettingsPageModel model)
        {
            _model = model;
            _model.LanguageChanged += LanguageChangedHandler;
        }

        public void Init()
        {
            GameObject selfObject = _model.SelfObject;
            Transform selfTransform = selfObject.transform;
            Transform panelTransform = selfTransform.Find("Image_Panel");

            Button exitButton = panelTransform.Find("Image_PanelClose/Button_Exit").GetComponent<Button>();
            Button aboutUsButton = panelTransform.Find("Button_AboutUs").GetComponent<Button>();
            Button tutorialButton = panelTransform.Find("Button_Tutorial").GetComponent<Button>();
            Button languageNextButton = panelTransform.Find("Container_Language/Button_Next").GetComponent<Button>();
            Button languagePreviousButton = panelTransform.Find("Container_Language/Button_Previous").GetComponent<Button>();

            Slider musicSlider = panelTransform.Find("Container_Music/Slider_Value").GetComponent<Slider>();
            Slider soundsSlider = panelTransform.Find("Container_Sound/Slider_Value").GetComponent<Slider>();

            exitButton.onClick.AddListener(ExitButtonOnClick);
            aboutUsButton.onClick.AddListener(AboutUsButtonOnClick);
            tutorialButton.onClick.AddListener(TutorialButtonOnClick);
            languageNextButton.onClick.AddListener(LanguageNextButtonOnClick);
            languagePreviousButton.onClick.AddListener(LanguagePreviusButtonOnClick);

            musicSlider.onValueChanged.AddListener(MusicSliderValueChanged);
            soundsSlider.onValueChanged.AddListener(SoundSliderValueChanged);
        }

        private void LanguageChangedHandler()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            GameObject selfObject = _model.SelfObject;

            TextMeshProUGUI pageTitle = selfObject.transform.
                Find("Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI aboutUsButtonTitle = selfObject.transform.
                Find("Image_Panel/Button_AboutUs/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI tutorialButtonTitle = selfObject.transform.
                Find("Image_Panel/Button_Tutorial/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI musicTitle = selfObject.transform.
                Find("Image_Panel/Container_Music/Text_Main").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI soundTitle = selfObject.transform.
                Find("Image_Panel/Container_Sound/Text_Main").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI languageTitle = selfObject.transform.
                Find("Image_Panel/Container_Language/Text_Main").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI currentLanguageTitle = selfObject.transform.
                Find("Image_Panel/Container_Language/Image_CurrentLanguageBackground/Text_Main").GetComponent<TextMeshProUGUI>();

            pageTitle.text = _model.GetLocalisation("KEY_SETTINGS_TITLE");
            aboutUsButtonTitle.text = _model.GetLocalisation("KEY_ABOUT_US_TITLE");
            tutorialButtonTitle.text = _model.GetLocalisation("KEY_TUTORIAL_TITLE");
            musicTitle.text = _model.GetLocalisation("KEY_SETTINGS_MUSIC");
            soundTitle.text = _model.GetLocalisation("KEY_SETTINGS_SOUNDS");
            languageTitle.text = _model.GetLocalisation("KEY_SETTINGS_LANGUAGE");
            currentLanguageTitle.text = _model.GetLocalisation("KEY_SETTINGS_SELECTED_LANGUAGE");
        }

        public void Dispose()
        {
        }

        public void Show(object data = null)
        {
            _model.SelfObject.SetActive(true);
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
        }

        private void ExitButtonOnClick()
        {
            _model.OpenMainMenu();
        }

        private void AboutUsButtonOnClick()
        {
            Debug.LogWarning("Not implemented");
        }

        private void TutorialButtonOnClick()
        {
            Debug.LogWarning("Not implemented");
        }

        private void LanguageNextButtonOnClick()
        {
            _model.NextLocalisation();
        }

        private void LanguagePreviusButtonOnClick()
        {
            _model.PreviousLocalisation();
        }

        private void MusicSliderValueChanged(float value)
        {
            Debug.LogWarning("Not implemented");
        }

        private void SoundSliderValueChanged(float value)
        {
            Debug.LogWarning("Not implemented");
        }
    }
}
