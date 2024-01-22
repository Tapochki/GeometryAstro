using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.SceneSystems;
using ChebDoorStudio.UI.Views.Base;
using ChebDoorStudio.Utilities;
using UnityEngine.UI;
using Zenject;

namespace ChebDoorStudio.UI.Views
{
    public class ViewGameOverPage : View
    {
        private ShadowedTextMexhProUGUI _scoreText;
        private ShadowedTextMexhProUGUI _coinsText;
        private Button _returnToMenuButton;

        private SceneSystem _sceneSystem;
        private PauseSystem _pauseSystem;
        private DataSystem _dataSystem;
        private LocalisationSystem _localisationSystem;
        private GameStateSystem _gameStateSystem;
        private VaultSystem _vaultSystem;

        [Inject]
        public void Construct(SceneSystem sceneSystem, PauseSystem pauseSystem,
                                LocalisationSystem localisationSystem, DataSystem dataSystem,
                                GameStateSystem gameStateSystem, VaultSystem vaultSystem)
        {
            _sceneSystem = sceneSystem;
            _pauseSystem = pauseSystem;
            _localisationSystem = localisationSystem;
            _dataSystem = dataSystem;
            _gameStateSystem = gameStateSystem;
            _vaultSystem = vaultSystem;

            _vaultSystem.OnCoinsAmountChangedEvent += OnCoinsAmountChangedEventHandler;
        }

        public override void Initialize()
        {
            _scoreText = transform.Find("ShadowedText_Score").GetComponent<ShadowedTextMexhProUGUI>();
            _coinsText = transform.Find("ShadowedText_Coin").GetComponent<ShadowedTextMexhProUGUI>();

            _returnToMenuButton = transform.Find("Button_Exit").GetComponent<Button>();

            base.Initialize();

            _coinsText.UpdateTextAndShadowValue($"{_localisationSystem.GetString("title_coins")}: {_vaultSystem.Coins.Get()}");

            _returnToMenuButton.onClick.AddListener(ReturnToMenuButtonOnClickHandler);
        }

        public override void Show()
        {
            base.Show();

            _pauseSystem.SetOn();

            //_scoreText.UpdateTextAndShadowValue($"{_localisationSystem.GetString("title_score")}: {_scoreSystem.GetScore()}");
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
            _dataSystem = null;
            _localisationSystem = null;

            _scoreText = null;
            _coinsText = null;

            _returnToMenuButton.onClick.RemoveListener(ReturnToMenuButtonOnClickHandler);
            _returnToMenuButton = null;
        }

        private void OnCoinsAmountChangedEventHandler(int amount)
        {
            _coinsText.UpdateTextAndShadowValue($"{_localisationSystem.GetString("title_coins")}: {amount}");
        }

        private void ReturnToMenuButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _gameStateSystem.GameplayStoped();

            _gameStateSystem.ChangeGameState(Settings.GameStates.Menu);

            _pauseSystem.SetOff();

            _sceneView.HideView();

            _sceneView.GameStoped();
        }
    }
}