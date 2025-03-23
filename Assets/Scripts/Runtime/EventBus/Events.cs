namespace TandC.GeometryAstro.EventBus
{
    public readonly struct PlayerHealthChangeEvent : IEvent
    {
        public readonly int CurrentHealth;
        public readonly float MaxHealth;

        public PlayerHealthChangeEvent(int currentHealt, float maxHealth)
        {
            CurrentHealth = currentHealt;
            MaxHealth = maxHealth;
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

    public readonly struct PlayerLevelUpEvent : IEvent
    {
        public readonly int CurrentLevel;

        public PlayerLevelUpEvent(int currentLevel)
        {
            CurrentLevel = currentLevel;
        }
    }
}