using TandC.UI.Views.Base;
using TandC.Utilities;
using UnityEngine;
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

        private ShadowedTextMexhProUGUI _currentLanguageTitleText;

        [Inject]
        public void Construct()
        {
        }

        public override void Initialize()
        {
            base.Initialize();
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
        }

        public override void Update()
        {
            base.Update();
        }
    }
}