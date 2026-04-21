using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class MoneyVaultContainer : IEventReceiver<CoinItemReleaseEvent>
    {
        public UniqueId Id { get; } = new UniqueId();

        public int CurrentMoneyCount { get; private set; }

        private IReadableModificator _moneyModificator;

        private readonly MoneyVaultView _moneyVaultView;

        public MoneyVaultContainer()
        {
            _moneyVaultView = GameObject.FindAnyObjectByType<MoneyVaultView>();
            _moneyVaultView.Init();

            RegisterEvent();
        }

        public void Init(IReadableModificator moneyModificator) 
        {
            _moneyModificator = moneyModificator;
        }

        private void Dispose() 
        {
            UnregisterEvent();
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CoinItemReleaseEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CoinItemReleaseEvent>);
        }

        public void OnEvent(CoinItemReleaseEvent @event)
        {
            AddMoney(@event.CoinAmount);
        }

        public void AddMoney(int baseAmount)
        {
            CurrentMoneyCount += Mathf.RoundToInt(baseAmount * _moneyModificator.Value);
            _moneyVaultView.UpdateText(CurrentMoneyCount);
        }
    }
}

