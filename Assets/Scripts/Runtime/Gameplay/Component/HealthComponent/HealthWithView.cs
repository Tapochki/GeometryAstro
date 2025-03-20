using System;

namespace TandC.GeometryAstro.Gameplay 
{
    public class HealthWithView : IHealth
    {
        private IHealth _baseHealth;
        public Action<float, float> OnHealthChangeEvent { get; protected set; }

        public float MaxHealth => _baseHealth.MaxHealth;
        public float CurrentHealth => _baseHealth.CurrentHealth;

        public HealthWithView(IHealth baseHealth, Action<float, float> onHealthChangeEvent)
        {
            _baseHealth = baseHealth;
            OnHealthChangeEvent = onHealthChangeEvent;
            OnHealthChangeEvent?.Invoke(CurrentHealth, MaxHealth);
        }

        public void TakeDamage(float amount)
        {
            _baseHealth.TakeDamage(amount);
            OnHealthChangeEvent?.Invoke(CurrentHealth, MaxHealth);
        }

        public void Heal(float amount)
        {
            _baseHealth.Heal(amount);
            OnHealthChangeEvent?.Invoke(CurrentHealth, MaxHealth);
        }
    }
}

