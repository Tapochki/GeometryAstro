using TandC.GeometryAstro.EventBus;

namespace TandC.GeometryAstro.Gameplay
{
    public class LevelModel : IEventReceiver<ExpirienceItemReleaseEvent>
    {
        private int _currentLevel;

        private float _currentXp;
        private float _xpForNextLevel;

        private IReadableModificator _expModificator;

        public UniqueId Id { get; } = new UniqueId();

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this);
        }

        public void OnEvent(ExpirienceItemReleaseEvent @event)
        {
            AddExpirience(@event.ExpAmount);
        }

        public void Init(IReadableModificator expModificator)
        {
            _expModificator = expModificator;

            InitEvent();
            SetStartLevel();
            SetStartExpirience();
        }

        private void InitEvent()
        {
            RegisterEvent();
        }

        private void Dispose()
        {
            UnregisterEvent();
        }

        private void SetStartLevel()
        {
            _currentLevel = 1;
            UpdateView();
        }

        private void SetStartExpirience()
        {
            _xpForNextLevel = 0;
            _xpForNextLevel = 100;
            UpdateView();
        }

        public void AddExpirience(int addedXp)
        {
            _currentXp += addedXp;
            CheckForNewLevel();
            UpdateView();
        }

        private void UpdateView()
        {
            EventBusHolder.EventBus.Raise(new ExpirienceChangeEvent(_currentXp, _xpForNextLevel, _currentLevel));
        }

        public void CheckForNewLevel()
        {
            if (_currentXp >= _xpForNextLevel)
            {
                _currentXp -= _xpForNextLevel;
                MuliplyExpirienceForNewLevel();
                LevelUp();
            }
        }

        private void MuliplyExpirienceForNewLevel()
        {
            _xpForNextLevel *= _expModificator.Value;
        }

        private void LevelUp()
        {
            _currentLevel++;
            UpdateView();
            EventBusHolder.EventBus.Raise(new PlayerLevelUpEvent(_currentLevel));
            //_skillService.StartGenerateSkills();
        }

    }
}


