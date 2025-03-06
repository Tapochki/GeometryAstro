
using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class MainMenuPageView : IUIPage
    {
        public bool IsActive { get; private set; }

        private MainMenuPageModel _model;

        private Button _startutton;

        private CanvasGroup _canvasGroup;

        public MainMenuPageView(MainMenuPageModel model)
        {
            _model = model;
        }

        public void Init()
        {
            GameObject selfObject = _model.GetSelfObject();
            Transform selfTransform = selfObject.transform;

            _canvasGroup = selfObject.GetComponent<CanvasGroup>();

            _startutton = selfTransform.Find("Button_StartGame").GetComponent<Button>();

            _startutton.onClick.AddListener(StartButtonOnClick);
        }

        public void Dispose()
        {
            Debug.Log("menu dispose");
        }

        public void Show(object data = null)
        {
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void StartButtonOnClick()
        {
            _model.LoadGameScene();
            // TODO - play ClickSound
        }
    }
}