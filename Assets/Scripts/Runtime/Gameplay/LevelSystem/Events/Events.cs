using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.EventBus
{
    public readonly struct ActiveSkillUpgradeEvent : IEvent
    {
        public readonly ActiveSkillType ActiveSkillType;

        public ActiveSkillUpgradeEvent(ActiveSkillType activeSkillType)
        {
            ActiveSkillType = activeSkillType;
        }
    }

    public readonly struct PassiveSkillUpgradeEvent : IEvent
    {
        public readonly ModificatorType PassiveSkillType;

        public PassiveSkillUpgradeEvent(ModificatorType passiveSkillType)
        {
            PassiveSkillType = passiveSkillType;
        }
    }

    public readonly struct ActiveSkillEvolveEvent: IEvent
    {
        public readonly ActiveSkillType ActiveSkillType;

        public ActiveSkillEvolveEvent(ActiveSkillType activeSkillType)
        {
            ActiveSkillType = activeSkillType;
        }
    }
}