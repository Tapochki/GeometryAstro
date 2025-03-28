using Cysharp.Threading.Tasks;
using System;
using VContainer;

namespace TandC.GeometryAstro.Services
{
    public sealed class VaultService: ILoadUnit
    {
        public event Action<int> OnCoinsAmountChangedEvent;

        private DataService _dataService;

        public VaultCoin Coins { get; private set; }

        [Inject]
        public void Construct(DataService dataService)
        {
            _dataService = dataService;
        }

        public async UniTask Load()
        {
            Initialize();
            await UniTask.CompletedTask;
        }

        public void Initialize()
        {
            Coins = new VaultCoin(_dataService, OnCoinsAmountChangedEventHandler);
        }

        private void OnCoinsAmountChangedEventHandler()
        {
            OnCoinsAmountChangedEvent?.Invoke(Coins.Get());
        }

        public class VaultCase
        {
            private Action _changedVaultAction;

            private DataService _dataService;

            private int _storedItem;

            public VaultCase(DataService dataService, Action changedVaultAction)
            {
                _dataService = dataService;

                _storedItem = _dataService.PlayerVaultData.coins;
                _changedVaultAction = changedVaultAction;
            }

            public void Add(int amount)
            {
                _storedItem += amount;
                _changedVaultAction?.Invoke();

                _dataService.PlayerVaultData.coins = _storedItem;
                _dataService.SaveCache(Settings.CacheType.PlayerValutData);
            }

            public void Substruct(int amount)
            {
                _storedItem -= amount;
                _changedVaultAction?.Invoke();

                _dataService.PlayerVaultData.coins = _storedItem;
                _dataService.SaveCache(Settings.CacheType.PlayerValutData);
            }

            public int Get() => _storedItem;
        }

        public sealed class VaultCoin : VaultCase
        {
            public VaultCoin(DataService dataSystem, Action changedVaultAction) : base(dataSystem, changedVaultAction)
            {
            }
        }
    }
}