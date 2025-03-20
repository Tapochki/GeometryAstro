using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ModifiableHealth : IHealth
    {
        private HealthWithView _baseHealth;

        private IReadableModificator _armorModificator; 
        private IReadableModificator _maxHealthModificator;

        public float MaxHealth => _baseHealth.MaxHealth + _maxHealthModificator.Value;
        public float CurrentHealth => _baseHealth.CurrentHealth;

        public ModifiableHealth(HealthWithView baseHealth, IReadableModificator armorModificator, IReadableModificator maxHealthModificator)
        {
            _baseHealth = baseHealth;
            _armorModificator = armorModificator;
            _maxHealthModificator = maxHealthModificator;

            _maxHealthModificator.OnValueChanged += UpdateMaxHealth;
        }

        public void TakeDamage(float amount)
        {
            float armor = Mathf.Clamp01(_armorModificator.Value);
            float reducedDamage = amount * (1 - armor);
            _baseHealth.TakeDamage(reducedDamage);
        }

        public void Heal(float amount)
        {
            _baseHealth.Heal(amount);
        }

        private void UpdateMaxHealth(float newValue)
        {
            _baseHealth.OnHealthChangeEvent?.Invoke(CurrentHealth, MaxHealth);
        }
    }
}

