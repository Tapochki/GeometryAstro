using UnityEngine;
using UnityEngine.UI;

namespace TandC.GeometryAstro.UI
{
    public class GamePageView : IUIPage
    {
        public bool IsActive { get; private set; }

        private GamePageModel _model;

        public GamePageView(GamePageModel model)
        {
            _model = model;
        }

        public void Init()
        {
            GameObject selfObject = _model.SelfObject;
            Transform selfTransform = selfObject.transform;

            Button pauseButton = selfTransform.Find("GamePage/Container_TopPanel/Button_Pause").GetComponent<Button>();

            pauseButton.onClick.AddListener(PauseButtonOnClick);
        }

        public void Dispose()
        {
            _model.Dispose();
        }

        public void Show(object data = null)
        {
            _model.SelfObject.SetActive(true);
        }

        public void Hide()
        {
            _model.SelfObject.SetActive(false);
        }

        private void PauseButtonOnClick()
        {
            _model.OpenPause();
        }
    }
}