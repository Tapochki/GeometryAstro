using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class SkillFactory : MonoBehaviour, ISkillFactory
    {
        private EventBusHolder _eventBusHolder;

        public SkillFactory(EventBusHolder eventBusHolder)
        {
            _eventBusHolder = eventBusHolder;
        }

        public Skill CreateSkill(SkillData skillData)
        {
            return skillData.useType switch
            {
                SkillUseType.Active => CreateActiveSkill(skillData),
                SkillUseType.Passive => CreatePassiveSkill(skillData),
                _ => throw new ArgumentException()
            };
        }

        private ActiveSkill CreateActiveSkill(SkillData skillData)
        {
            return skillData.SkillDescription.type switch
            {
                SkillType.StandartGun => new StandartGunSkill(skillData, _eventBusHolder),
                SkillType.Rocket => new RocketLauncherSkill(skillData, _eventBusHolder),
                SkillType.Dash => new DashSkill(skillData, _eventBusHolder),
                SkillType.Mask => new MaskSkill(skillData, _eventBusHolder),
                _ => null
            };
        }

        private PassiveSkill CreatePassiveSkill(SkillData skillData)
        {
            return skillData.SkillDescription.type switch
            {
                SkillType.MaxHealthIncrease => new HealthUpgradeSkill(skillData, _eventBusHolder),
                SkillType.MovementSpeedIncrease => new SpeedUpgradeSkill(skillData, _eventBusHolder),
                SkillType.Armor => new ArmorUpgradeSkill(skillData, _eventBusHolder),
                _ => null
            };
        }
    }
}

