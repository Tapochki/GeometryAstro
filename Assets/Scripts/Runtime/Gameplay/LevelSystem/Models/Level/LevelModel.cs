using TandC.GeometryAstro.EventBus;

namespace TandC.GeometryAstro.Gameplay
{
    public class LevelModel : IEventReceiver<ExperienceItemReleaseEvent>
    {
        private int _currentLevel;

        private float _currentXp;
        private float _xpForNextLevel;

        private IReadableModificator _expModificator;

        private const float _experienceNextLevelMultiplier = 1f;

        public UniqueId Id { get; } = new UniqueId();

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this);
        }

        public void OnEvent(ExperienceItemReleaseEvent @event)
        {
            AddExperience(@event.ExpAmount);
        }

        public void Init(IReadableModificator expModificator)
        {
            _expModificator = expModificator;

            InitEvent();
            SetStartLevel();
            SetStartExperience();
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

        private void SetStartExperience()
        {
            _xpForNextLevel = 0;
            _xpForNextLevel = 100;
            UpdateView();
        }

        public void AddExperience(int addedXp)
        {
            _currentXp += addedXp * _expModificator.Value;
            CheckForNewLevel();
            UpdateView();
        }

        private void UpdateView()
        {
            EventBusHolder.EventBus.Raise(new ExperienceChangeEvent(_currentXp, _xpForNextLevel, _currentLevel));
        }

        public void CheckForNewLevel()
        {
            if (_currentXp >= _xpForNextLevel)
            {
                _currentXp -= _xpForNextLevel;
                MultiplyExperienceForNewLevel();
                LevelUp();
            }
        }

        private void MultiplyExperienceForNewLevel()
        {
            _xpForNextLevel *= _experienceNextLevelMultiplier;
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


