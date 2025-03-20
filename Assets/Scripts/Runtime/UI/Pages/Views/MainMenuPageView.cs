using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class MainMenuPageView : IUIPage
    {
        public bool IsActive { get; private set; }

        private MainMenuPageModel _model;

        public MainMenuPageView(MainMenuPageModel model)
        {
            _model = model;
            _model.LanguageChanged += LanguageChangedHandler;
        }

        public void Init()
        {
            GameObject selfObject = _model.SelfObject;
            Transform selfTransform = selfObject.transform;
            Transform containerButtons = selfTransform.Find("Container_Buttons");

            Button startButton = containerButtons.Find("Button_Start").GetComponent<Button>();
            Button shopButton = containerButtons.Find("Button_Shop").GetComponent<Button>();
            Button leaderstatsButton = containerButtons.Find("Button_Leaderboard").GetComponent<Button>();
            Button settingsButton = selfTransform.Find("Container_SafeArea/Button_Settings").GetComponent<Button>();

            startButton.onClick.AddListener(StartButtonOnClick);
            shopButton.onClick.AddListener(ShopButtonOnClick);
            leaderstatsButton.onClick.AddListener(LeaderstatsButtonOnClick);
            settingsButton.onClick.AddListener(SettingsButtonOnClick);

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

            TextMeshProUGUI startButtonTitle = selfObject.transform.
                Find("Container_Buttons/Button_Start/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI shopButtonTitle = selfObject.transform.
                Find("Container_Buttons/Button_Shop/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI leaderboardButtonTitle = selfObject.transform.
                Find("Container_Buttons/Button_Leaderboard/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI infoDescriptionTitle = selfObject.transform.
                Find("Container_Advice/Container_Info/Text_Description").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI infoTitle = selfObject.transform.
                Find("Container_Advice/Container_Info/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI noSignalTitle = selfObject.transform.
                Find("Container_Advice/Container_Info/Container_NoSignal/Text_Title").GetComponent<TextMeshProUGUI>();

            startButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_START");
            shopButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_SHOP");
            leaderboardButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_LEADERBOARD");
            infoDescriptionTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_VALUE_0");
            infoTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_TITLE");
            noSignalTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_NO_SIGNAL");
        }

        public void Show(object data = null)
        {
            _model.SelfObject.SetActive(true);
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
        }

        private void StartButtonOnClick()
        {
            _model.LoadGameScene();
            // TODO - play ClickSound
        }

        private void ShopButtonOnClick()
        {
            //_model.OpenShop();
            // TODO - play ClickSound
        }

        private void SettingsButtonOnClick()
        {
            _model.OpenSetting();
            // TODO - play ClickSound
        }

        private void LeaderstatsButtonOnClick()
        {
            //_model.OpenLeaderstats();
            // TODO - play ClickSound
        }
    }
}