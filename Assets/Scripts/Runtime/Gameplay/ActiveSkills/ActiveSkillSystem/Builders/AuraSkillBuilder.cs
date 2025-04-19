using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class AuraSkillBuilder : ActiveSkillBuilder<AuraSkill>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.AuraGun;

        private Transform _playerTransformSkills;

        public AuraSkillBuilder(Transform playerTransformSkills) 
        {
            _playerTransformSkills = playerTransformSkills;
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            _skill.SetReloader(new SkillReloader(_activeSkillData.shootDeley, reloadModificator));
        }

        private void SetSkillPrefab()
        {
            _skill.SetObject(_playerTransformSkills);
        }

        private void SetSkillView() 
        {
            _skill.SetView(
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier),
                _modificatorContainer.GetModificator(ModificatorType.BulletsSize)
                );
        }

        protected override void ConstructSkill()
        {
            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));

            SetSkillPrefab();

            SetSkillView();
        }
    }
}

