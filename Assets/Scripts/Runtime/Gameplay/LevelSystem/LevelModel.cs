using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class LevelModel : MonoBehaviour
    {
        private const float _expirienceMultiplayer = 1.5f; //TODO to config

        [SerializeField]
        private LevelView _levelView;

        private int _currentLevel;

        private float _currentXp;
        private float _xpForNextLevel;

        private ISkillService _skillService;

        private void Construct(ISkillService skillService)
        {
            _skillService = skillService;
        }

        private void Start() 
        {
            SetStartLevel();
            SetStartExpirience();
        }

        private void SetStartLevel() 
        {
            _currentLevel = 1;
            UpdateViewLevel();
        }

        private void SetStartExpirience()
        {
            _xpForNextLevel = 0;
            _xpForNextLevel = 100;
            UpdateViewExpririence();
        }

        public void AddExpirience(int addedXp)
        {
            _currentXp += addedXp;
            CheckForNewLevel();
            UpdateViewExpririence();
        }

        private void UpdateViewExpririence()
        {
            _levelView.UpdateExpririence(_currentXp, _xpForNextLevel);
        }

        private void UpdateViewLevel() 
        {
            _levelView.UpdateLevel(_currentLevel);
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
            UpdateViewLevel();
            _skillService.StartGenerateSkills();
        }
    }
}


