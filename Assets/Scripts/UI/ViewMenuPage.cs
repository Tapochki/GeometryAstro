using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.UI.Views.Base;
using UnityEngine.UI;
using Zenject;

namespace ChebDoorStudio.UI.Views
{
    public sealed class ViewMenuPage : View
    {
        private Button _startButton;

        private SceneSystem _sceneSystems;
        private VaultSystem _vaultSystem;

        [Inject]
        public void Construct(SceneSystem sceneSystems, VaultSystem vaultSystem)
        {
            _sceneSystems = sceneSystems;
            _vaultSystem = vaultSystem;

            _vaultSystem.OnCoinsAmountChangedEvent += OnCoinsAmountChangedEventHandler;
        }

        public override void Initialize()
        {
            _startButton = transform.Find("Button_Start").GetComponent<Button>();

            base.Initialize();

            _startButton.onClick.AddListener(StartButtonOnClickHandler);
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

            _startButton = null;

            _sceneSystems = null;
        }

        private void StartButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _sceneSystems.OpenLoadedScene();
        }

        private void OnCoinsAmountChangedEventHandler(int amount)
        {
        }
    }
}