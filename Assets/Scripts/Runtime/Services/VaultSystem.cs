using System;
using UnityEngine;

namespace TandC.GeometryAstro.Services
{
    public sealed class VaultSystem : MonoBehaviour
    {
        public event Action<int> OnCoinsAmountChangedEvent;

        private DataSystem _dataSystem;
        private GameStateSystem _gameStateSystem;

        public VaultCoin Coins { get; private set; }

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

                _storedItem = _dataSystem.PlayerVaultData.coins;
                _changedVaultAction = changedVaultAction;
            }

            public void Add(int amount)
            {
                _storedItem += amount;
                _changedVaultAction?.Invoke();

                _dataSystem.PlayerVaultData.coins = _storedItem;
                _dataSystem.SaveCache(Settings.CacheType.PlayerValutData);
            }

            public void Substruct(int amount)
            {
                _storedItem -= amount;
                _changedVaultAction?.Invoke();

                _dataSystem.PlayerVaultData.coins = _storedItem;
                _dataSystem.SaveCache(Settings.CacheType.PlayerValutData);
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