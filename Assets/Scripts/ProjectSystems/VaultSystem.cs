using System;
using Zenject;

namespace Studio.ProjectSystems
{
    public sealed class VaultSystem : IInitializable
    {
        public event Action<int> OnCoinsAmountChangedEvent;

        private DataSystem _dataSystem;
        private GameStateSystem _gameStateSystem;

        public VaultCoin Coins { get; private set; }

        [Inject]
        public void Construct(DataSystem dataSystem, GameStateSystem gameStateSystem)
        {
            _dataSystem = dataSystem;
        }

        public void Initialize()
        {
            Coins = new VaultCoin(_dataSystem, OnCoinsAmountChangedEventHandler);
        }

        private void OnCoinsAmountChangedEventHandler()
        {
            OnCoinsAmountChangedEvent?.Invoke(Coins.Get());
        }

        public class VaultCase
        {
            private Action _changedVaultAction;

            private DataSystem _dataSystem;

            private int _storedItem;

            public VaultCase(DataSystem dataSystem, Action changedVaultAction)
            {
                _dataSystem = dataSystem;

                _changedVaultAction = changedVaultAction;
            }

            public void Add(int amount)
            {
                _storedItem += amount;
                _changedVaultAction?.Invoke();


            }

            public void Substruct(int amount)
            {
                _storedItem -= amount;
                _changedVaultAction?.Invoke();


            }

            public int Get() => _storedItem;
        }

        public sealed class VaultCoin : VaultCase
        {
            public VaultCoin(DataSystem dataSystem, Action changedVaultAction) : base(dataSystem, changedVaultAction)
            {
            }
        }
    }
}