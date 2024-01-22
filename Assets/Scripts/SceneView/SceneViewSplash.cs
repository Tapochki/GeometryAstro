using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.Scenes.Base;
using ChebDoorStudio.Settings;
using ChebDoorStudio.UI.Views.Base;
using System.Collections.Generic;
using Zenject;

namespace ChebDoorStudio.Scenes
{
    public class SceneViewSplash : SceneView
    {
        private GameStateSystem _gameStateSystem;
        private SceneSystem _sceneSystem;

        [Inject]
        public void Construct(GameStateSystem gameStateSystem, SceneSystem sceneSystem)
        {
            _gameStateSystem = gameStateSystem;
            _sceneSystem = sceneSystem;

            _sceneSystem.LoadSceneByName(Settings.SceneNames.Loading, Settings.SceneNames.Game);
        }

        [Inject]
        public override void Initialize()
        {
            _menuRootView = transform.Find("View - SplashPage").GetComponent<View>();
            _views = new List<View>();

            base.Initialize();

            _gameStateSystem.ChangeGameState(GameStates.Splash);

            _gameStateSystem.WorkerUninitialized();
        }

        public override void ShowView(View view)
        {
            base.ShowView(view);
        }

        public override void HideView()
        {
            base.HideView();
        }

        public override void Dispose()
        {
            base.Dispose();

            _gameStateSystem = null;
            _sceneSystem = null;

            //EventBus.OnSceneSystemsBindedEvent -= OnSceneSystemsBindedEventHandler;
        }
    }
}