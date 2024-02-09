using System;
using TandC.UI.Views.Base;
using TandC.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TandC.UI.Views
{
    public class ViewGameOverPage : View
    {
        private Button _continueButton;
        private Button _reviewButton;

        [Inject]
        public void Construct()
        {
        }

        public override void Initialize()
        {
            _continueButton = transform.Find("Image_Background/Button_Continue").GetComponent<Button>();
            _reviewButton = transform.Find("Image_Background/Button_Review").GetComponent<Button>();

            base.Initialize();

            _continueButton.onClick.AddListener(ContinueButtonOnClickHandler);
            _reviewButton.onClick.AddListener(ReviewButtonOnClickHandler);
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

            _continueButton.onClick.RemoveListener(ContinueButtonOnClickHandler);
            _reviewButton.onClick.RemoveListener(ReviewButtonOnClickHandler);

            _continueButton = null;
            _reviewButton = null;
        }

        public override void Update()
        {
            base.Update();
        }

        private void ReviewButtonOnClickHandler()
        {
            TandC.Utilities.Logger.NotImplementedLog("Review in GameOverPage"); // TODO - add watch ad handler and review player
        }

        private void ContinueButtonOnClickHandler()
        {
            _sceneView.HideView();
            TandC.Utilities.Logger.NotImplementedLog("ContinueButton in GameOverPage"); // TODO - add Pause Off and return to menu handler
        }
    }
}