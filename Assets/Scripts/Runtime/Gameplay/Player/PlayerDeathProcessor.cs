using System;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.UI;
using TandC.GeometryAstro.Utilities;
using UniRx;
using VContainer;

namespace TandC.GeometryAstro.Gameplay 
{
    public class PlayerDeathProcessor : IEventReceiver<PlayerDieEvent>
    {
        private const float FREEZE_TIME_AFTER_REVIVE_TIMER = 3f;
        public UniqueId Id { get; } = new UniqueId();

        private Player _player;
        private UIService _uIService;

        private IReadableModificator _maxRevives;
        private IReadableModificator _maxHealth;
        private int _playerDeathCount;
        private float _reviveDelay = 2f;

        [Inject]
        private void Construct(Player player, UIService uIService) 
        {
            _player = player;
            _uIService = uIService;
        }

        public void Init(IReadableModificator maxHealth, IReadableModificator maxRevives)
        {
            _maxRevives = maxRevives;
            _maxHealth = maxHealth;
            _playerDeathCount = 0;

            RegisterEvent();

        }

        public void Dispose() 
        {
            UnregisterEvent();
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<PlayerDieEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<PlayerDieEvent>);
        }

        public void OnEvent(PlayerDieEvent @event)
        {
            _playerDeathCount++;
            if (_playerDeathCount <= _maxRevives.Value)
            {
                _player.PlayerDisable();
                StartReviveTimer();
            }
            else
            {
                PlayerDeathProcess();
            }
        }
        private void PlayerDeathProcess() 
        {
            _uIService.OpenPage<GameOverPageView>();
            _player.PlayerDisable();
        }

        private CompositeDisposable _reviveDisposables = new CompositeDisposable();

        public void StartReviveTimer()
        {
            Observable.Timer(TimeSpan.FromSeconds(_reviveDelay))
                .Subscribe(_ => RevivePlayer())
                .AddTo(_reviveDisposables);
        }

        private void RevivePlayer()
        {
            _player.PlayerEnable();

            float reviveHealHealth = _maxHealth.Value / 2;

            EventBusHolder.EventBus.Raise(new PlayerHealReleaseEvent((int)reviveHealHealth));
            EventBusHolder.EventBus.Raise(new FrozeBombReleaseEvent(FREEZE_TIME_AFTER_REVIVE_TIMER));
            
        }
    }
}

