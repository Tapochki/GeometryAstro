using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ModifiableHealth : HealthWithView
    {
        private IReadableModificator _armorModificator; 
        private IReadableModificator _maxHealthModificator;

        public ModifiableHealth(float maxHealth, Action<bool> onDeathEvent, Action<int, float> onHealthChangeEvent, IReadableModificator maxHealthModificator, IReadableModificator armorModificator) : base(maxHealth, onDeathEvent, onHealthChangeEvent)
        {
            _armorModificator = armorModificator;
            _maxHealthModificator = maxHealthModificator;
            _maxHealthModificator.OnValueChanged = UpdateMaxHealth;
        }

        public override void TakeDamage(float amount)
        {
            float armor = Mathf.Clamp(_armorModificator.Value, 0, 100) / 100f;
            float reducedDamage = amount * (1 - armor);
            base.TakeDamage(reducedDamage);
        }

        private void UpdateMaxHealth(float additionalValue)
        {
            _maxHealth += additionalValue;
            base.Heal(additionalValue);
        }
    }
}

