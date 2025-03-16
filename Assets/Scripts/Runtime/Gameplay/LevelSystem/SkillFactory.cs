using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class SkillFactory : MonoBehaviour, ISkillFactory
    {

        public SkillFactory()
        {
            
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
                SkillType.StandartGun => new StandartGunSkill(skillData),
                SkillType.Rocket => new RocketLauncherSkill(skillData),
                SkillType.Dash => new DashSkill(skillData),
                SkillType.Mask => new MaskSkill(skillData),
                _ => null
            };
        }

        private PassiveSkill CreatePassiveSkill(SkillData skillData)
        {
            return skillData.SkillDescription.type switch
            {
                SkillType.MaxHealthIncrease => new HealthUpgradeSkill(skillData),
                SkillType.MovementSpeedIncrease => new SpeedUpgradeSkill(skillData),
                SkillType.Armor => new ArmorUpgradeSkill(skillData),
                _ => null
            };
        }
    }
}

