using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class LevelModel : IEventReceiver<ExpirienceItemReleaseEvent>
    {
        private const float _expirienceMultiplayer = 1.5f; //TODO to config

        private int _currentLevel;

        private float _currentXp;
        private float _xpForNextLevel;

        public UniqueId Id { get; } = new UniqueId();

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<ExpirienceItemReleaseEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<ExpirienceItemReleaseEvent>);
        }

        public void OnEvent(ExpirienceItemReleaseEvent @event)
        {
            AddExpirience(@event.ExpAmount);
        }

        public void Init()
        {
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
            _xpForNextLevel *= _expirienceMultiplayer;
        }

        private void LevelUp() 
        {
            _currentLevel++;
            UpdateView();
            //_skillService.StartGenerateSkills();
        }

    }
}


