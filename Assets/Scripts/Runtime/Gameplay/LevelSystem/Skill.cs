using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay
{
    public abstract class Skill
    {
        public int SkillLevel { get; private set; }

        protected SkillData _skillData;

        protected EventBusHolder _eventBusHolder;

        protected IEvent _activateSkillEvent;

        public Skill(SkillData skillData, EventBusHolder eventBusHolder)
        {
            _skillData = skillData;
            _eventBusHolder = eventBusHolder;
        }

        public virtual void ApplySkillEffect()
        {
            UpgradeLevel();
        }

        public SkillDescription GetUpgradeData()
        {
            SkillDescription skillDescription = _skillData.UpgradeList[SkillLevel].SkillDescription;
            return skillDescription;
        }

        public SkillType GetSkillType()
        {
            return _skillData.SkillDescription.type;
        }

        private void UpgradeLevel()
        {
            SkillLevel++;
        }

        public bool IsMaxLevel()
        {
            return SkillLevel <= _skillData.MaxLevel;
        }
    }

    public abstract class ActiveSkill : Skill
    {
        public ActiveSkill(SkillData skillData, EventBusHolder eventBusHolder) : base(skillData, eventBusHolder) { }

        //For future evolution
        public void CheckEvolution() { }

        public void Evolution() { }
    }

    public abstract class PassiveSkill : Skill
    {
        public PassiveSkill(SkillData skillData, EventBusHolder eventBusHolder) : base(skillData, eventBusHolder) { }
    }

    public class HealthUpgradeSkill : PassiveSkill
    {
        public HealthUpgradeSkill(SkillData skillData, EventBusHolder eventBusHolder) : base(skillData, eventBusHolder) { }

        public override void ApplySkillEffect() 
        {
            _eventBusHolder.EventBus.Raise(new HealthSkillUpgradeEvent());
            base.ApplySkillEffect();
        }
    }

    public class SpeedUpgradeSkill : PassiveSkill
    {
        public SpeedUpgradeSkill(SkillData skillData, EventBusHolder eventBusHolder) : base(skillData, eventBusHolder) { }

        public override void ApplySkillEffect()
        {
            _eventBusHolder.EventBus.Raise(new SpeedSkillUpgradeEvent());
            base.ApplySkillEffect();
        }
    }

    public class ArmorUpgradeSkill : PassiveSkill
    {
        public ArmorUpgradeSkill(SkillData skillData, EventBusHolder eventBusHolder) : base(skillData, eventBusHolder) { }

        public override void ApplySkillEffect()
        {
            _eventBusHolder.EventBus.Raise(new ArmorSkillUpgradeEvent());
            base.ApplySkillEffect();
        }
    }

    public class StandartGunSkill : ActiveSkill
    {
        public StandartGunSkill(SkillData skillData, EventBusHolder eventBusHolder) : base(skillData, eventBusHolder) { }

        public override void ApplySkillEffect()
        {
            _eventBusHolder.EventBus.Raise(new StandartGunSkillEvent());
            base.ApplySkillEffect();
        }
    }

    public class RocketLauncherSkill : ActiveSkill 
    {
        public RocketLauncherSkill(SkillData skillData, EventBusHolder eventBusHolder) : base(skillData, eventBusHolder) { }

        public override void ApplySkillEffect()
        {
            _eventBusHolder.EventBus.Raise(new RocketLaunherSkillEvent());
            base.ApplySkillEffect();
        }
    }

    public class DashSkill : ActiveSkill
    {
        public DashSkill(SkillData skillData, EventBusHolder eventBusHolder) : base(skillData, eventBusHolder) { }

        public override void ApplySkillEffect()
        {
            _eventBusHolder.EventBus.Raise(new DashSkillEvent());
            base.ApplySkillEffect();
        }
    }

    public class MaskSkill : ActiveSkill
    {
        public MaskSkill(SkillData skillData, EventBusHolder eventBusHolder) : base(skillData, eventBusHolder) { }

        public override void ApplySkillEffect()
        {
            _eventBusHolder.EventBus.Raise(new MaskSkillEvent());
            base.ApplySkillEffect();
        }
    }
}

