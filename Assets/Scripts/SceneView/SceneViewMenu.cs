using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.Scenes.Base;
using ChebDoorStudio.Settings;
using ChebDoorStudio.UI.Views.Base;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ChebDoorStudio.Scenes
{
    public class SceneViewMenu : SceneView
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

            _sceneSystem.LoadSceneByName(Settings.SceneNames.Loading, Settings.SceneNames.Game);
        }

        [Inject]
        public override void Initialize()
        {
            _rootView = transform.Find("View - MenuPage").GetComponent<View>();
            _views = new List<View>();

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

            _gameStateSystem = null;
            _sceneSystem = null;
        }
    }
}