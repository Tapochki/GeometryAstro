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
            Transform containerButtons = selfTransform.Find("Image_Background");

            Button continueButton = containerButtons.Find("Button_Continue").GetComponent<Button>();
            Button oneMoreButton = containerButtons.Find("Button_OneMore ").GetComponent<Button>();

            continueButton.onClick.AddListener(ContinueButtonOnClick);
            oneMoreButton.onClick.AddListener(OneMoreButtonOnClick);

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

            TextMeshProUGUI continueButtonTitle = selfObject.transform.
                Find("Image_Background/Button_Continue/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI oneMoreButtonTitle = selfObject.transform.
                Find("Image_Background/Button_OneMore/Text_Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI titleTitle = selfObject.transform.
                Find("Image_Background/Text_Title/Text_Title").GetComponent<TextMeshProUGUI>();

            continueButtonTitle.text = _model.GetLocalisation("KEY_CONTINUE");
            oneMoreButtonTitle.text = _model.GetLocalisation("KEY_GAMEOVER_ONE_MORE_CHANCE");
            titleTitle.text = _model.GetLocalisation("KEY_GAMEOVER_TITLE");
        }

        public void Show(object data = null)
        {
            _model.SelfObject.SetActive(true);

            GameObject selfObject = _model.SelfObject;

            TextMeshProUGUI coinsText = selfObject.transform.
                Find("Image_Background/Coin_Panel/Text_Main").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI recordText = selfObject.transform.
                Find("Image_Background/Record_Panel/Text_Main").GetComponent<TextMeshProUGUI>();
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
        }

        private void ContinueButtonOnClick()
        {
            // TODO - play ClickSound
            _model.Continue();
        }

        private void OneMoreButtonOnClick()
        {
            _model.OneMoreChance();
            // TODO - play ClickSound
        }
    }
}