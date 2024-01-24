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

        protected GameObject _gameObject;

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
            _gameObject.SetActive(false);
            _onDeathEvent?.Invoke();
        }
    }

    public class PlayerHealthComponent : HealthComponent
    {
        public PlayerHealthComponent(float maxHealth, Action onDeathEvent, Action<float> onHealthChangeEvent) : base(maxHealth, onDeathEvent, null)
        {
            this._onHealthChageEvent = () => onHealthChangeEvent?.Invoke(_currentHealth);
        }

        public void Heal(float amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        }

        protected override void Die()
        {
            base.Die();
        }
    }
}

