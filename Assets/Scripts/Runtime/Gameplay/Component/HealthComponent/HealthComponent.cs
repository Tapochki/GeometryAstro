using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class HealthComponent : IHealth
    {
        public virtual float MaxHealth { get; protected set; }
        public virtual float CurrentHealth { get; protected set; }

        protected Action<bool> _onDeathEvent;

        public HealthComponent(float maxHealth, Action<bool> onDeathEvent) 
        {
            MaxHealth = CurrentHealth = maxHealth;
            _onDeathEvent = onDeathEvent;
        }

        public virtual void TakeDamage(float amount)
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth -= amount;
                if (CurrentHealth <= 0)
                {
                    Die();
                }
            }
        }

        public virtual void Heal(float amount) 
        {
            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        }

        protected virtual void Die()
        {
            _onDeathEvent?.Invoke(true);
        }
    }
}

