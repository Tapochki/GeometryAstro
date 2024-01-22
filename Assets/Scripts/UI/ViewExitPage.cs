using ChebDoorStudio.UI.Views.Base;
using UnityEngine;
using UnityEngine.UI;

namespace ChebDoorStudio.UI.Views
{
    public class ViewExitPage : View
    {
        private Button _yesButton;
        private Button _noButton;

        public override void Initialize()
        {
            _yesButton = transform.Find("Button_Yes").GetComponent<Button>();
            _noButton = transform.Find("Button_No").GetComponent<Button>();

            base.Initialize();

            _yesButton.onClick.AddListener(YesButtonOnClickHandler);
            _noButton.onClick.AddListener(NoButtonOnClickHandler);
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

            _yesButton.onClick.RemoveListener(YesButtonOnClickHandler);
            _noButton.onClick.RemoveListener(NoButtonOnClickHandler);

            _yesButton = null;
            _noButton = null;
        }

        private void YesButtonOnClickHandler()
        {
            ChebDoorStudio.Utilities.Logger.Log("Quit from application", Settings.LogTypes.Info);
            _soundSystem.PlayClickSound();
            Application.Quit();
        }

        private void NoButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();
            _sceneView.HideView();
        }
    }
}