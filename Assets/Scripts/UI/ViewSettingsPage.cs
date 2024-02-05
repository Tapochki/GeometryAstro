using System;
using TandC.ProjectSystems;
using TandC.Settings;
using TandC.UI.Views.Base;
using TandC.Utilities;
using UnityEngine.UI;
using Zenject;

namespace TandC.UI.Views
{
    public class ViewSettingsPage : View
    {
        private Button _closeButton;
        private Button _nextLanguageButton;
        private Button _previousLanguageButton;
        private Button _aboutUsButton;
        private Button _tutorialButton;

        private Slider _musicSlider;
        private Slider _soundSlider;

        private ShadowedTextMexhProUGUI _currentLanguageTitleText;

        private DataSystem _dataSystem;
        private LocalisationSystem _localisationSystem;

        private int _languageIndex;
        private int _totalLanguagesIndex;

        [Inject]
        public void Construct(DataSystem dataSystem, LocalisationSystem localisationSystem)
        {
            _dataSystem = dataSystem;
            _localisationSystem = localisationSystem;
        }

        public override void Initialize()
        {
            _closeButton = transform.Find("Image_Panel/Image_PanelClose/Button_Exit").GetComponent<Button>();
            _nextLanguageButton = transform.Find("Image_Panel/Container_Language/Button_Next").GetComponent<Button>();
            _previousLanguageButton = transform.Find("Image_Panel/Container_Language/Button_Previous").GetComponent<Button>();
            _aboutUsButton = transform.Find("Image_Panel/Button_AboutUs").GetComponent<Button>();
            _tutorialButton = transform.Find("Image_Panel/Button_Tutorial").GetComponent<Button>();

            _musicSlider = transform.Find("Image_Panel/Container_Music/Slider_Value").GetComponent<Slider>();
            _soundSlider = transform.Find("Image_Panel/Container_Sound/Slider_Value").GetComponent<Slider>();

            _currentLanguageTitleText = transform.Find("Image_Panel/Container_Language/Image_CurrentLanguageBackground/ShadowedText_CurrentLanguage").GetComponent<ShadowedTextMexhProUGUI>();

            base.Initialize();

            _totalLanguagesIndex = Enum.GetValues(typeof(Languages)).Length;

            _musicSlider.value = _dataSystem.AppSettingsData.musicVolume;
            _soundSlider.value = _dataSystem.AppSettingsData.soundVolume;

            _musicSlider.onValueChanged.AddListener(MusicSliderOnValuerChangedEvent);
            _soundSlider.onValueChanged.AddListener(SoundSliderOnValuerChangedEvent);

            _closeButton.onClick.AddListener(CloseButtonOnClickHandler);
            _nextLanguageButton.onClick.AddListener(NextButtonOnClickHandler);
            _previousLanguageButton.onClick.AddListener(PreviousButtonOnClickHandler);
            _aboutUsButton.onClick.AddListener(AboutButtonOnClickHandler);
            _tutorialButton.onClick.AddListener(TutorialButtonOnClickHandler);
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();

            _dataSystem.SaveCache(CacheType.AppSettingsData);
        }

        public override void Dispose()
        {
            base.Dispose();

            _musicSlider.onValueChanged.RemoveListener(MusicSliderOnValuerChangedEvent);
            _soundSlider.onValueChanged.RemoveListener(SoundSliderOnValuerChangedEvent);

            _closeButton.onClick.RemoveListener(CloseButtonOnClickHandler);
            _nextLanguageButton.onClick.RemoveListener(NextButtonOnClickHandler);
            _previousLanguageButton.onClick.RemoveListener(PreviousButtonOnClickHandler);
            _aboutUsButton.onClick.RemoveListener(AboutButtonOnClickHandler);
            _tutorialButton.onClick.RemoveListener(TutorialButtonOnClickHandler);

            _closeButton = null;
            _nextLanguageButton = null;
            _previousLanguageButton = null;
            _aboutUsButton = null;
            _tutorialButton = null;

            _musicSlider = null;
            _soundSlider = null;

            _currentLanguageTitleText = null;

            _dataSystem = null;
            _localisationSystem = null;
        }

        public override void Update()
        {
            base.Update();
        }

        private void MusicSliderOnValuerChangedEvent(float value)
        {
            _musicSlider.value = value;

            _dataSystem.AppSettingsData.musicVolume = value;
        }

        private void SoundSliderOnValuerChangedEvent(float value)
        {
            _soundSlider.value = value;

            _dataSystem.AppSettingsData.soundVolume = value;
        }

        private void UpdateCurrentLanguageTitle()
        {
            switch (_languageIndex)
            {
                case 0:
                    _currentLanguageTitleText.UpdateTextAndShadowValue("English");
                    _localisationSystem.UpdateLocalisation(Languages.English);
                    _dataSystem.AppSettingsData.appLanguage = Languages.English;
                    break;

                case 1:
                    _currentLanguageTitleText.UpdateTextAndShadowValue("Українська");
                    _localisationSystem.UpdateLocalisation(Languages.Ukrainian);
                    _dataSystem.AppSettingsData.appLanguage = Languages.Ukrainian;
                    break;

                case 2:
                    _currentLanguageTitleText.UpdateTextAndShadowValue("Русский");
                    _localisationSystem.UpdateLocalisation(Languages.Russian);
                    _dataSystem.AppSettingsData.appLanguage = Languages.Russian;
                    break;
            }
        }

        private void CloseButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneView.HideView();
        }

        private void NextButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _languageIndex++;

            if (_languageIndex >= _totalLanguagesIndex)
            {
                _languageIndex = 0;
            }

            UpdateCurrentLanguageTitle();
        }

        private void PreviousButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _languageIndex--;

            if (_languageIndex < 0)
            {
                _languageIndex = _totalLanguagesIndex - 1;
            }

            UpdateCurrentLanguageTitle();
        }

        private void AboutButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneView.ShowView<ViewAboutPage>();
        }

        private void TutorialButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            //_sceneView.ShowView<ViewTutorialPage>();
        }
    }
}