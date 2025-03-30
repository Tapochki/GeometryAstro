using UnityEngine;

namespace TandC.GeometryAstro.EventBus
{
    public readonly struct PlayerHealthChangeEvent : IEvent
    {
        public readonly int CurrentHealth;
        public readonly float MaxHealth;

        public readonly float ChangedValue;
        public readonly bool IsDamageOrHeal;

        public PlayerHealthChangeEvent(int currentHealth, float maxHealth, float changedValue, bool isDamageOrHeal)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            ChangedValue = changedValue;
            IsDamageOrHeal = isDamageOrHeal;
        }
    }

    public readonly struct PauseGameEvent : IEvent
    {
        public readonly bool SetPause;

        public PauseGameEvent(bool setPause)
        {
            SetPause = setPause;
        }

    }

    public readonly struct ExpirienceChangeEvent : IEvent
    {
        public readonly float CurrentExpirience;
        public readonly float MaxExpirienceForNextLevel;
        public readonly int CurrentLevel;

        public ExpirienceChangeEvent(float currentExpirience, float maxExpirienceForNextLevel, int currentLevel)
        {
            CurrentExpirience = currentExpirience;
            MaxExpirienceForNextLevel = maxExpirienceForNextLevel;
            CurrentLevel = currentLevel;
        }
    }

    public readonly struct PlayerDieEvent : IEvent { }
    public readonly struct TeleportPlayerToTheBoundaryEvent : IEvent { }

    public readonly struct PlayerLevelUpEvent : IEvent
    {
        public readonly int CurrentLevel;

        public PlayerLevelUpEvent(int currentLevel)
        {
            CurrentLevel = currentLevel;
        }
    }

    public readonly struct CreateExplosion : IEvent
    {
        public readonly Vector3 Position;
        public readonly float Radius;

        public CreateExplosion(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }
    }
}