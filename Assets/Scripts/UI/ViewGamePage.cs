using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.SceneSystems;
using ChebDoorStudio.UI.Views.Base;
using ChebDoorStudio.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ChebDoorStudio.UI.Views
{
    public class ViewGamePage : View
    {
        private ShadowedTextMexhProUGUI _scoreText;
        private ShadowedTextMexhProUGUI _coinsText;
        private ShadowedTextMexhProUGUI _timeToStartGameText;
        private Button _pauseButton;

        private Button _changeDirectionButton;

        private LocalisationSystem _localisationSystem;
        private GameStateSystem _gameStateSystem;
        private InputsSystem _inputSystem;
        private VaultSystem _vaultSystem;
        private PauseSystem _pauseSystem;

        private float _timeToStartGame;
        private bool _isGameStarted;

        private string _scoreTitle;

        [Inject]
        public void Construct(LocalisationSystem localisationSystem, GameStateSystem gameStateSystem, InputsSystem inputsSystem,
                                VaultSystem vaultSystem, PauseSystem pauseSystem)
        {
            _localisationSystem = localisationSystem;
            _gameStateSystem = gameStateSystem;
            _inputSystem = inputsSystem;
            _vaultSystem = vaultSystem;
            _pauseSystem = pauseSystem;

            _vaultSystem.OnCoinsAmountChangedEvent += OnCoinsAmountChangedEventHandler;
            _gameStateSystem.OnGameplayStopedEvent += OnGameplayStopedEventHandler;
            _pauseSystem.OnGameplayPausedEvent += OnGameplayPausedEventHandler;
        }

        public override void Initialize()
        {
            _changeDirectionButton = transform.Find("Button_ChangeDirection").GetComponent<Button>();

            _scoreText = transform.Find("Container_Score/ShadowedText_Value").GetComponent<ShadowedTextMexhProUGUI>();
            _timeToStartGameText = transform.Find("ShadowedText_TimeToStart").GetComponent<ShadowedTextMexhProUGUI>();
            _coinsText = transform.Find("Container_SafeArea/Container_Coins/ShadowedText_Value").GetComponent<ShadowedTextMexhProUGUI>();

            _pauseButton = transform.Find("Container_SafeArea/Button_Pause").GetComponent<Button>();

            base.Initialize();

            _pauseButton.onClick.AddListener(PauseButtonOnClickHandler);
            _changeDirectionButton.onClick.AddListener(ChangeDirectionButtonOnClickHandler);

            _scoreTitle = _localisationSystem.GetString("title_score");

            _scoreText.UpdateTextAndShadowValue($"{_scoreTitle}: {0}");
            _coinsText.UpdateTextAndShadowValue(_vaultSystem.Coins.Get().ToString());

            _isGameStarted = true;
        }

        public override void Update()
        {
            base.Update();

            if (!_isGameStarted)
            {
                _timeToStartGame -= Time.deltaTime;

                _timeToStartGameText.UpdateTextAndShadowValue($"{Mathf.Floor(_timeToStartGame)}");

                if (_timeToStartGame <= 0)
                {
                    _isGameStarted = true;
                    _timeToStartGameText.UpdateTextAndShadowValue("0");
                    _timeToStartGameText.gameObject.SetActive(false);
                    _gameStateSystem.GameplayStarted();
                }
            }
        }

        public override void Show()
        {
            base.Show();

            _isGameStarted = false;
            _timeToStartGame = 4.0f;
            _timeToStartGameText.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Dispose()
        {
            base.Dispose();

            _pauseButton.onClick.RemoveListener(PauseButtonOnClickHandler);
            _changeDirectionButton.onClick.RemoveListener(() => _inputSystem.UpdateMovementDirection());

            _scoreText = null;
            _timeToStartGameText = null;

            _pauseButton = null;
            _changeDirectionButton = null;

            _localisationSystem = null;
            _gameStateSystem = null;
            _inputSystem = null;
        }

        private void OnGameplayPausedEventHandler(bool isPaused)
        {
        }

        private void OnCoinsAmountChangedEventHandler(int amount)
        {
            _coinsText.UpdateTextAndShadowValue(amount.ToString());
        }

        private void OnGameplayStopedEventHandler()
        {
            _isGameStarted = true;
        }

        private void ChangeDirectionButtonOnClickHandler()
        {
            _inputSystem.UpdateMovementDirection();
        }

        private void PauseButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneView.ShowView<ViewPausePage>();
        }

        private void OnScoreUpdateEventHandler(int score)
        {
            _scoreText.UpdateTextAndShadowValue($"{_scoreTitle}: {score}");
        }
    }
}