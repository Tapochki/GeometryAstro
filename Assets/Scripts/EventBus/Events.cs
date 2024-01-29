using UnityEngine;
namespace TandC.EventBus 
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

    public readonly struct PlayerDieEvent : IEvent
    {

    }
}

