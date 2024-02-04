using TandC.ProjectSystems;
using TandC.UI.Views.Base;
using TandC.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TandC.UI.Views
{
    public sealed class ViewMenuPage : View
    {
        private Button _startButton;
        private Button _settingsButton;
        private Button _shopButton;
        private Button _leadeboardButton;

        private Animator _adviceAnimator;

        private ShadowedTextMexhProUGUI _adviceDescriptionText;

        private float _hideAdviceCooldown = 20.0f;
        private float _showAdviceCooldown = 10.0f;
        private float _adviceCooldown;

        private bool _isShowAdvice;
        private bool _isHideAdvice;

        private int _adviceIndex;

        private LocalisationSystem _localisationSystem;
        private SceneSystem _sceneSystems;
        private VaultSystem _vaultSystem;

        [Inject]
        public void Construct(SceneSystem sceneSystems, VaultSystem vaultSystem, LocalisationSystem localisationSystem)
        {
            _sceneSystems = sceneSystems;
            _vaultSystem = vaultSystem;
            _localisationSystem = localisationSystem;

            _vaultSystem.OnCoinsAmountChangedEvent += OnCoinsAmountChangedEventHandler;
        }

        public override void Initialize()
        {
            _startButton = transform.Find("Container_Buttons/Button_Start").GetComponent<Button>();
            _settingsButton = transform.Find("Container_SafeArea/Button_Settings").GetComponent<Button>();
            _shopButton = transform.Find("Container_Buttons/Button_Shop").GetComponent<Button>();
            _leadeboardButton = transform.Find("Container_Buttons/Button_Leaderboard").GetComponent<Button>();

            _adviceDescriptionText = transform.Find("Container_Advice/Container_Info/ShadowedText_Description").GetComponent<ShadowedTextMexhProUGUI>();

            _adviceAnimator = transform.Find("Container_Advice").GetComponent<Animator>();

            base.Initialize();

            _adviceIndex = 0;

            _startButton.onClick.AddListener(StartButtonOnClickHandler);
            _settingsButton.onClick.AddListener(SettingsButtonOnClickHandler);
            _shopButton.onClick.AddListener(ShopButtonOnClickHandler);
            _leadeboardButton.onClick.AddListener(LeaderbordButtonOnClickHandler);
        }

        private void ResetAdviceIndex()
        {
            if (_adviceIndex >= 4)
            {
                _adviceIndex = 0;
            }
        }

        public override void Show()
        {
            base.Show();

            ResetAdviceIndex();
            UpdateAdvice();
            _adviceCooldown = _hideAdviceCooldown;
            _isHideAdvice = true;
            _isShowAdvice = false;
        }

        public override void Hide()
        {
            base.Hide();

            _isHideAdvice = false;
            _isShowAdvice = false;
        }

        public override void Dispose()
        {
            base.Dispose();

            _startButton.onClick.RemoveListener(StartButtonOnClickHandler);

            _startButton = null;

            _sceneSystems = null;
        }

        public override void Update()
        {
            base.Update();

            if (_isHideAdvice)
            {
                _adviceCooldown -= Time.deltaTime;

                if (_adviceCooldown <= 0)
                {
                    _isHideAdvice = false;
                    _adviceAnimator.Play("Hide", -1, 0);
                    _adviceCooldown = _showAdviceCooldown;
                    _isShowAdvice = true;
                }
            }

            if (_isShowAdvice)
            {
                _adviceCooldown -= Time.deltaTime;

                if (_adviceCooldown <= 0)
                {
                    _isShowAdvice = false;
                    ResetAdviceIndex();
                    UpdateAdvice();
                    _adviceCooldown = _hideAdviceCooldown;
                    _isHideAdvice = true;
                }
            }
        }

        private void UpdateAdvice()
        {
            _adviceDescriptionText.UpdateTextAndShadowValue(_localisationSystem.GetString($"key_advice_desctiption_{_adviceIndex}"));

            _adviceAnimator.Play("Show", -1, 0);
            _adviceIndex++;
        }

        private void StartButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneSystems.OpenLoadedScene();
        }

        private void SettingsButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneView.ShowView<ViewSettingsPage>();
        }

        private void ShopButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneView.ShowView<ViewSelectShopPage>();
        }

        private void LeaderbordButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();
        }

        private void OnCoinsAmountChangedEventHandler(int amount)
        {
        }
    }
}