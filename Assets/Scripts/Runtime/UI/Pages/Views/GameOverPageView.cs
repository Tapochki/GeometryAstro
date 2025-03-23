using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class GameOverPageView : IUIPage
    {
        public bool IsActive { get; private set; }

        private GameOverPageModel _model;

        public GameOverPageView(GameOverPageModel model)
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

            startButton.onClick.AddListener(StartButtonOnClick);
            shopButton.onClick.AddListener(ShopButtonOnClick);

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

            startButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_START");
            shopButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_SHOP");
            leaderboardButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_LEADERBOARD");
            infoDescriptionTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_VALUE_0");
            infoTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_TITLE");
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
            // TODO - play ClickSound
        }

        private void ShopButtonOnClick()
        {
            //_model.OpenShop();
            // TODO - play ClickSound
        }

        private void SettingsButtonOnClick()
        {
            // TODO - play ClickSound
        }

        private void LeaderstatsButtonOnClick()
        {
            //_model.OpenLeaderstats();
            // TODO - play ClickSound
        }
    }
}