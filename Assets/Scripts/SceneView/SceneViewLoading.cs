using System.Collections.Generic;
using TandC.ProjectSystems;
using TandC.Scenes.Base;
using TandC.Settings;
using TandC.UI.Views.Base;
using Zenject;

namespace TandC.Scenes
{
    public class SceneViewLoading : SceneView
    {
        private GameStateSystem _gameStateSystem;

        [Inject]
        public void Construct(GameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;
        }

        [Inject]
        public override void Initialize()
        {
            _rootView = transform.Find("View - LoadingPage").GetComponent<View>();
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
        }
    }
}