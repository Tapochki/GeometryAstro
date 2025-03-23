using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class BaseHealthComponent : IHealth
    {
        protected float _maxHealth;
        protected float _currentHealth;

        protected Action<bool> _onDeathEvent;

        public BaseHealthComponent(float maxHealth, Action<bool> onDeathEvent) 
        {
            _currentHealth = _maxHealth = maxHealth;
            _onDeathEvent = onDeathEvent;
        }

        public virtual void TakeDamage(float amount)
        {
            if (_currentHealth > 0)
            {
                _currentHealth -= amount;
                if (_currentHealth <= 0)
                {
                    Die();
                }
            }
        }

        public virtual void Heal(float amount) 
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        }

        protected virtual void Die()
        {
            _onDeathEvent?.Invoke(true);
        }
    }
}

