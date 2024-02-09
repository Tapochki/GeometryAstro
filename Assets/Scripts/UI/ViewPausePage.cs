using TandC.ProjectSystems;
using TandC.UI.Views.Base;
using TandC.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TandC.UI.Views
{
    public class ViewPausePage : View
    {
        private ShadowedTextMexhProUGUI _scoreText;
        private Button _exitButton;
        private Button _continueButton;
        private Button _settingsButton;

        private LocalisationSystem _localisationSystem;

        [Inject]
        public void Construct(LocalisationSystem localisationSystem)
        {
            _localisationSystem = localisationSystem;
        }

        public override void Initialize()
        {
            _scoreText = transform.Find("Image_Background/ShadowedText_Score").GetComponent<ShadowedTextMexhProUGUI>();

            _exitButton = transform.Find("Image_Background/Button_Exit").GetComponent<Button>();
            _continueButton = transform.Find("Image_Background/Button_Continue").GetComponent<Button>();
            _settingsButton = transform.Find("Image_Background/Button_Settings").GetComponent<Button>();

            base.Initialize();

            _exitButton.onClick.AddListener(ExitButtonOnClickHandler);
            _continueButton.onClick.AddListener(ContinueButtonOnClickHandler);
            _settingsButton.onClick.AddListener(SettingsButtonOnClickHandler);
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Dispose()
        {
            base.Dispose();

            _exitButton.onClick.RemoveListener(ExitButtonOnClickHandler);
            _continueButton.onClick.RemoveListener(ContinueButtonOnClickHandler);
            _settingsButton.onClick.RemoveListener(SettingsButtonOnClickHandler);

            _scoreText = null;
            _exitButton = null;
            _continueButton = null;
            _settingsButton = null;
            _localisationSystem = null;
        }

        public override void Update()
        {
            base.Update();
        }

        public void UpdateScoreText(int value)
        {
            _scoreText.UpdateTextAndShadowValue($"{_localisationSystem.GetString("key_score_title")}: {value}");
        }

        private void ContinueButtonOnClickHandler()
        {
            _sceneView.HideView();
            TandC.Utilities.Logger.NotImplementedLog("ContinueButton in PausePage"); // TODO - add PauseOff handler
        }

        private void ExitButtonOnClickHandler()
        {
            TandC.Utilities.Logger.NotImplementedLog("ExitButton in PausePage"); // TODO - add return to menu scene and stop gameplay
        }

        private void SettingsButtonOnClickHandler()
        {
            _sceneView.ShowView<ViewSettingsPage>();
        }
    }
}