using Studio.ProjectSystems;
using Studio.Scenes.Base;
using Studio.Settings;
using Studio.UI.Views.Base;
using System.Collections.Generic;
using Zenject;

namespace Studio.Scenes
{
    public class SceneViewLoading : SceneView
    {
        private GameStateSystem _gameStateSystem;

        [Inject]
        public void Construct(GameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;

            //EventBus.OnSceneSystemsBindedEvent += OnSceneSystemsBindedEventHandler;
        }

        [Inject]
        public override void Initialize()
        {
            _menuRootView = transform.Find("View - LoadingPage").GetComponent<View>();
            _views = new List<View>();

            base.Initialize();

            _gameStateSystem.ChangeGameState(GameStates.Loading);

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

            //EventBus.OnSceneSystemsBindedEvent -= OnSceneSystemsBindedEventHandler;
        }

        private void OnSceneSystemsBindedEventHandler()
        {
            Initialize();
        }
    }
}