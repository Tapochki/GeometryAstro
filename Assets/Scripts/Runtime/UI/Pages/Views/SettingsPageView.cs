using TMPro;
using UnityEngine;

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
        }

        private void LanguageChangedHandler()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            GameObject selfObject = _model.SelfObject;

            //TextMeshProUGUI startButtonTitle = selfObject.transform.
            //    Find("Container_Buttons/Button_Start/Text_Title").GetComponent<TextMeshProUGUI>();
            //TextMeshProUGUI shopButtonTitle = selfObject.transform.
            //    Find("Container_Buttons/Button_Shop/Text_Title").GetComponent<TextMeshProUGUI>();
            //TextMeshProUGUI leaderboardButtonTitle = selfObject.transform.
            //    Find("Container_Buttons/Button_Leaderboard/Text_Title").GetComponent<TextMeshProUGUI>();
            //TextMeshProUGUI infoDescriptionTitle = selfObject.transform.
            //    Find("Container_Advice/Container_Info/Text_Description").GetComponent<TextMeshProUGUI>();
            //TextMeshProUGUI infoTitle = selfObject.transform.
            //    Find("Container_Advice/Container_Info/Text_Title").GetComponent<TextMeshProUGUI>();
            //TextMeshProUGUI noSignalTitle = selfObject.transform.
            //    Find("Container_Advice/Container_Info/Container_NoSignal/Text_Title").GetComponent<TextMeshProUGUI>();

            //startButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_START");
            //shopButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_SHOP");
            //leaderboardButtonTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_LEADERBOARD");
            //infoDescriptionTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_VALUE_0");
            //infoTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_TITLE");
            //noSignalTitle.text = _model.GetLocalisation("KEY_MAIN_MENU_INFO_NO_SIGNAL");
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
    }
}
