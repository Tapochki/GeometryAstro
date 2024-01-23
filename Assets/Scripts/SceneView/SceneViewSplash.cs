using System.Collections.Generic;
using TandC.ProjectSystems;
using TandC.Scenes.Base;
using TandC.Settings;
using TandC.UI.Views.Base;
using Zenject;

namespace TandC.Scenes
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

            _sceneSystem.LoadSceneByName(Settings.SceneNames.Loading, Settings.SceneNames.Menu);
        }

        [Inject]
        public override void Initialize()
        {
            _rootView = transform.Find("View - SplashPage").GetComponent<View>();
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