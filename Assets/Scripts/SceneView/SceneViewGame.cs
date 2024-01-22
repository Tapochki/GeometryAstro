using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.Scenes.Base;
using ChebDoorStudio.Settings;
using ChebDoorStudio.UI.Views;
using ChebDoorStudio.UI.Views.Base;
using System.Collections.Generic;
using Zenject;

namespace ChebDoorStudio.Scenes
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
        }

        [Inject]
        public override void Initialize()
        {
            _menuRootView = transform.Find("View - MenuPage").GetComponent<View>();
            _gameRootView = transform.Find("View - GamePage").GetComponent<View>();
            _views = new List<View>()
            {
                transform.Find("View - PausePage").GetComponent<View>(),
                transform.Find("View - GameOverPage").GetComponent<View>(),
                transform.Find("View - ExitPage").GetComponent<View>(),
                transform.Find("View - ShopPage").GetComponent<View>(),
            };

            base.Initialize();

            _gameStateSystem.ChangeGameState(GameStates.Menu);

            _gameStateSystem.WorkerInitialized();

            _soundSystem.PlaySound(Sounds.Background);
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