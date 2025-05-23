using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.EventBus
{
    public readonly struct ActiveSkillUpgradeEvent : IEvent
    {
        public readonly ActiveSkillType ActiveSkillType;
        public readonly float UpgradeValue;

        public ActiveSkillUpgradeEvent(ActiveSkillType activeSkillType, float upgradeValue)
        {
            ActiveSkillType = activeSkillType;
            UpgradeValue = upgradeValue;
        }
    }

    public readonly struct PassiveSkillUpgradeEvent : IEvent
    {
        public readonly ModificatorType PassiveSkillType;
        public readonly float UpgradeValue;

        public PassiveSkillUpgradeEvent(ModificatorType passiveSkillType, float upgradeValue)
        {
            PassiveSkillType = passiveSkillType;
            UpgradeValue = upgradeValue;
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