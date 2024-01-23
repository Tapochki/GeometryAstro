using TandC.ProjectSystems;
using TandC.UI.Views.Base;
using UnityEngine.UI;
using Zenject;

namespace TandC.UI.Views
{
    public class ViewGamePage : View
    {
        private SceneSystem _sceneSystem;

        private Button _exitButton;

        [Inject]
        public void Construct(SceneSystem sceneSystem)
        {
            _sceneSystem = sceneSystem;
        }

        public override void Initialize()
        {
            _exitButton = transform.Find("Button_Exit").GetComponent<Button>();

            base.Initialize();

            _exitButton.onClick.AddListener(ExitButtonOnClickHandler);
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

        private void ExitButtonOnClickHandler()
        {
            _sceneSystem.OpenLoadedScene();
        }
    }
}