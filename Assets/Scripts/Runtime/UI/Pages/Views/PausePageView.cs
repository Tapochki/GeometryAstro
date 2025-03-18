using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class PausePageView : IUIPage
    {
        public bool IsActive { get; private set; }

        private PausePageModel _model;

        public PausePageView(PausePageModel model)
        {
            _model = model;
            _model.LanguageChanged += LanguageChangedHandler;
        }

        public void Init()
        {
            GameObject selfObject = _model.SelfObject;
            Transform selfTransform = selfObject.transform;
            Transform container = selfTransform.Find("Image_Background");

            Button continueButton = container.Find("Button_Continue").GetComponent<Button>();
            Button settingsButton = container.Find("Button_Settings").GetComponent<Button>();
            Button exitButton = container.Find("Button_Exit").GetComponent<Button>();

            continueButton.onClick.AddListener(ContinueButtonOnClick);
            settingsButton.onClick.AddListener(SettingsButtonOnClick);
            exitButton.onClick.AddListener(ExitButtonOnClick);

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

            TextMeshProUGUI continueButtonTitle = selfObject.transform.
                Find("Image_Background/Button_Continue/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI settingsButtonTitle = selfObject.transform.
                Find("Image_Background/Button_Settings/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI exitButtonTitle = selfObject.transform.
                Find("Image_Background/Button_Exit/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI titleTitle = selfObject.transform.
                Find("Image_Background/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI scoreTitle = selfObject.transform.
                Find("Image_Background/Text_ScoreTitle").GetComponent<TextMeshProUGUI>();

            continueButtonTitle.text = _model.GetLocalisation("KEY_CONTINUE");
            settingsButtonTitle.text = _model.GetLocalisation("KEY_SETTINGS_TITLE");
            exitButtonTitle.text = _model.GetLocalisation("KEY_EXIT");
            titleTitle.text = _model.GetLocalisation("KEY_PAUSE_TITLE");
            scoreTitle.text = _model.GetLocalisation("KEY_SCORE");
        }

        public void Show(object data = null)
        {
            _model.SelfObject.SetActive(true);
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
        }

        private void ContinueButtonOnClick()
        {
            _model.ContinueGame();
            // TODO - play ClickSound
        }

        private void SettingsButtonOnClick()
        {
            //_model.OpenSettings();
            // TODO - play ClickSound
        }

        private void ExitButtonOnClick()
        {
            _model.LoadMenuScene();
            // TODO - play ClickSound
        }
    }
}