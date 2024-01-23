using System.Collections.Generic;
using TandC.ProjectSystems;
using TandC.Scenes.Base;
using TandC.Settings;
using TandC.UI.Views.Base;
using Zenject;

namespace TandC.Scenes
{
    public class SceneViewGame : SceneView
    {
        private GameStateSystem _gameStateSystem;
        private SceneSystem _sceneSystem;
        private SoundSystem _soundSystem;

        [Inject]
        public void Construct(GameStateSystem gameStateSystem, SceneSystem sceneSystem, SoundSystem soundSystem)
        {
            _gameStateSystem = gameStateSystem;
            _sceneSystem = sceneSystem;
            _soundSystem = soundSystem;

            _sceneSystem.LoadSceneByName(SceneNames.Loading, SceneNames.Menu);
        }

        [Inject]
        public override void Initialize()
        {
            _rootView = transform.Find("View - GamePage").GetComponent<View>();

            _views = new List<View>()
            {
            };

            base.Initialize();

            _gameStateSystem.ChangeGameState(GameStates.Game);

            _gameStateSystem.WorkerInitialized();
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

            _sceneSystem = null;

            _gameStateSystem.GameplayStoped();

            _gameStateSystem.WorkerUninitialized();

            _gameStateSystem = null;
        }
    }
}