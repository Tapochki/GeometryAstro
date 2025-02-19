namespace TandC.GeometryAstro.EventBus
{
    public readonly struct PlayerHealthChangeEvent : IEvent
    {
        public readonly float CurrentHealth;
        public readonly float MaxHealth;

        public PlayerHealthChangeEvent(float currentHealt, float maxHealth)
        {
            CurrentHealth = currentHealt;
            MaxHealth = maxHealth;
        }
    }

    public readonly struct PlayerDieEvent : IEvent { }

    public readonly struct HealthSkillUpgradeEvent : IEvent { }

    public readonly struct SpeedSkillUpgradeEvent : IEvent { }

    public readonly struct ArmorSkillUpgradeEvent : IEvent { }

    public readonly struct RocketLaunherSkillEvent : IEvent { }

    public readonly struct MaskSkillEvent : IEvent { }

    public readonly struct DashSkillEvent : IEvent { }

    public readonly struct StandartGunSkillEvent: IEvent { }
}