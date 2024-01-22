using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.SceneSystems;
using ChebDoorStudio.UI.Views.Base;
using UnityEngine.UI;
using Zenject;

namespace ChebDoorStudio.UI.Views
{
    public class ViewPausePage : View
    {
        private Button _continueButton;
        private Button _returnToMenuButton;

        private SceneSystem _sceneSystem;
        private PauseSystem _pauseSystem;
        private GameStateSystem _gameStateSystem;

        [Inject]
        public void Construct(SceneSystem sceneSystem, PauseSystem pauseSystem, GameStateSystem gameStateSystem)
        {
            _sceneSystem = sceneSystem;
            _pauseSystem = pauseSystem;
            _gameStateSystem = gameStateSystem;
        }

        public override void Initialize()
        {
            _continueButton = transform.Find("Button_Continue").GetComponent<Button>();
            _returnToMenuButton = transform.Find("Button_Exit").GetComponent<Button>();

            base.Initialize();

            _continueButton.onClick.AddListener(ContinueButtonOnClickHandler);
            _returnToMenuButton.onClick.AddListener(ReturnToMenuButtonOnClickHandler);
        }

        public override void Show()
        {
            base.Show();

            _pauseSystem.SetOn();
        }

        public override void Hide()
        {
            base.Hide();

            _pauseSystem.SetOff();
        }

        public override void Dispose()
        {
            base.Dispose();

            _sceneSystem = null;
            _pauseSystem = null;

            _continueButton.onClick.RemoveListener(ContinueButtonOnClickHandler);
            _returnToMenuButton.onClick.RemoveListener(ReturnToMenuButtonOnClickHandler);

            _continueButton = null;
            _returnToMenuButton = null;

            _gameStateSystem = null;
        }

        public void ReturnToMenuButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _gameStateSystem.GameplayStoped();

            _pauseSystem.SetOff();

            _sceneView.HideView();

            _sceneView.GameStoped();
        }

        private void ContinueButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneView.HideView();
        }
    }
}