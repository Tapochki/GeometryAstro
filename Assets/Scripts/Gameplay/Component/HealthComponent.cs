using System;
using UnityEngine;

namespace TandC.Gameplay 
{
    public class HealthComponent
    {
        protected float _maxHealth;
        protected float _currentHealth;

        protected Action _onDeathEvent;
        protected Action _onHealthChageEvent;

        public HealthComponent(float maxHealth, Action onDeathEvent, Action onHealthChageEvent) 
        {
            _maxHealth = _currentHealth = maxHealth;
            _onDeathEvent = onDeathEvent;
            _onHealthChageEvent = onHealthChageEvent;
        }

        public void TakeDamage(float amount)
        {
            if (_currentHealth > 0)
            {
                _currentHealth -= amount;
                _onHealthChageEvent?.Invoke();
                if (_currentHealth <= 0)
                {
                    Die();
                }
            }
        }

        protected virtual void Die()
        {
            _onDeathEvent?.Invoke();
        }
    }

    public class HealthWithViewComponent : HealthComponent
    {
        public HealthWithViewComponent(float maxHealth, Action onDeathEvent, Action<float, float> onHealthChangeEvent) : base(maxHealth, onDeathEvent, null)
        {
            _onHealthChageEvent = () => onHealthChangeEvent?.Invoke(_currentHealth, _maxHealth);
            _onHealthChageEvent.Invoke();
        }
    }

    public class HealedHealthComponent : HealthWithViewComponent
    {
        public HealedHealthComponent(float maxHealth, Action onDeathEvent, Action<float, float> onHealthChangeEvent) : base(maxHealth, onDeathEvent, onHealthChangeEvent) { }
        public void Heal(float amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
            _onHealthChageEvent?.Invoke();
        }
    }
}

