using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.SceneSystems;
using ChebDoorStudio.UI.Views.Base;
using ChebDoorStudio.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ChebDoorStudio.UI.Views
{
    public sealed class ViewMenuPage : View
    {
        private Button _startButton;
        private Button _shopButton;
        private Button _exitButton;
        private AudioButtons _soundButton;
        private AudioButtons _musicButton;

        private ShadowedTextMexhProUGUI _coinsCountText;
        private ShadowedTextMexhProUGUI _bestResultText;

        private SceneSystem _sceneSystems;
        private DataSystem _dataSystem;
        private GameStateSystem _gameStateSystem;
        private LocalisationSystem _localisationSystem;
        private VaultSystem _vaultSystem;

        [Inject]
        public void Construct(SceneSystem sceneSystems, DataSystem dataSystem,
                                GameStateSystem gameStateSystem, LocalisationSystem localisationSystem, VaultSystem vaultSystem)
        {
            _sceneSystems = sceneSystems;
            _dataSystem = dataSystem;
            _gameStateSystem = gameStateSystem;
            _localisationSystem = localisationSystem;
            _vaultSystem = vaultSystem;

            _gameStateSystem.OnGameplayStopedEvent += OnGameplayStopedEventHandler;
            _vaultSystem.OnCoinsAmountChangedEvent += OnCoinsAmountChangedEventHandler;
        }

        public override void Initialize()
        {
            _startButton = transform.Find("Button_Start").GetComponent<Button>();
            _shopButton = transform.Find("Container_SafeArea/Button_Shop").GetComponent<Button>();
            _exitButton = transform.Find("Container_SafeArea/Button_Exit").GetComponent<Button>();

            _coinsCountText = transform.Find("Container_SafeArea/Container_Coins/ShadowedText_Value").GetComponent<ShadowedTextMexhProUGUI>();
            _bestResultText = transform.Find("ShadowedText_Score").GetComponent<ShadowedTextMexhProUGUI>();

            _soundButton = new AudioButtons(onButton: transform.Find("Container_SafeArea/Button_SoundOn").GetComponent<Button>(),
                                            offButton: transform.Find("Container_SafeArea/Button_SoundOff").GetComponent<Button>(),
                                            soundSystem: _soundSystem,
                                            type: AudioTypes.Sound);

            _musicButton = new AudioButtons(onButton: transform.Find("Container_SafeArea/Button_MusicOn").GetComponent<Button>(),
                                            offButton: transform.Find("Container_SafeArea/Button_MusicOff").GetComponent<Button>(),
                                            soundSystem: _soundSystem,
                                            type: AudioTypes.Music);

            base.Initialize();

            _soundButton.OnCacheLoaded(_soundSystem.SoundVolume == 1.0f);
            _musicButton.OnCacheLoaded(_soundSystem.MusicVolume == 1.0f);

            _coinsCountText.UpdateTextAndShadowValue(_vaultSystem.Coins.Get().ToString());

            _bestResultText.UpdateTextAndShadowValue($"{_localisationSystem.GetString("title_best_result")}: {_dataSystem.PlayerVaultData.bestScore}");

            _startButton.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f).SetLoops(-1);

            _startButton.onClick.AddListener(StartButtonOnClickHandler);
            _exitButton.onClick.AddListener(ExitButtonOnClickHandler);
            _shopButton.onClick.AddListener(ShopButtonOnClickHandler);
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Dispose()
        {
            base.Dispose();

            _startButton.onClick.RemoveListener(StartButtonOnClickHandler);
            _exitButton.onClick.RemoveListener(ExitButtonOnClickHandler);
            _shopButton.onClick.RemoveListener(ShopButtonOnClickHandler);

            _startButton = null;
            _shopButton = null;
            _exitButton = null;
            _coinsCountText = null;

            _soundButton.Dispose();
            _soundButton = null;
            _musicButton.Dispose();
            _musicButton = null;

            _sceneSystems = null;
            _localisationSystem = null;

            _coinsCountText = null;
        }

        private void StartButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _gameStateSystem.ChangeGameState(Settings.GameStates.Game);

            _sceneView.GameStarted();
        }

        private void ExitButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneView.ShowView<ViewExitPage>();
        }

        private void ShopButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneView.ShowView<ViewShopPage>();
        }

        private void OnBestScoreChangedEventHandler(int score)
        {
            _bestResultText.UpdateTextAndShadowValue($"{_localisationSystem.GetString("title_best_result")}: {score}");
        }

        private void OnGameplayStopedEventHandler()
        {
        }

        private void OnCoinsAmountChangedEventHandler(int amount)
        {
            _coinsCountText.UpdateTextAndShadowValue(amount.ToString());
        }

        internal enum AudioTypes
        {
            Music,
            Sound,
        }

        internal sealed class AudioButtons
        {
            private Button _onButton;
            private Button _offButton;

            private SoundSystem _soundSystem;

            private AudioTypes _type;

            private bool _isOn;

            public AudioButtons(Button onButton, Button offButton, SoundSystem soundSystem, AudioTypes type)
            {
                _soundSystem = soundSystem;

                _type = type;

                _onButton = onButton;
                _offButton = offButton;

                _onButton.onClick.AddListener(UpdateAudio);
                _offButton.onClick.AddListener(UpdateAudio);
            }

            public void OnCacheLoaded(bool isOn)
            {
                _isOn = isOn;

                UpdateVisability();
            }

            private void UpdateAudio()
            {
                _soundSystem.PlayClickSound();

                _isOn = !_isOn;

                UpdateVisability();

                switch (_type)
                {
                    case AudioTypes.Music:
                        _soundSystem.SetMusicVolume(_isOn ? 1.0f : 0.0f);
                        break;

                    case AudioTypes.Sound:
                        _soundSystem.SetSoundVolume(_isOn ? 1.0f : 0.0f);
                        break;

                    default:
                        ChebDoorStudio.Utilities.Logger.Log($"Audio type - [{_type}], not implemented!", Settings.LogTypes.Warning);
                        return;
                }

                _soundSystem.SaveData();
            }

            private void UpdateVisability()
            {
                _onButton.gameObject.SetActive(_isOn);
                _offButton.gameObject.SetActive(!_isOn);
            }

            public void Dispose()
            {
                _onButton.onClick.RemoveListener(UpdateAudio);
                _offButton.onClick.RemoveListener(UpdateAudio);

                _onButton = null;
                _offButton = null;
                _soundSystem = null;
            }
        }
    }
}