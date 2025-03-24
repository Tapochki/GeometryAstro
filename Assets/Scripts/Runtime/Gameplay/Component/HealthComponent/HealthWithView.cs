using System;

namespace TandC.GeometryAstro.Gameplay
{
    public class HealthWithView : BaseHealthComponent
    {
        public Action<int, float, float, bool> OnHealthChangeEvent { get; protected set; }

        public HealthWithView(float maxHealth, Action<bool> onDeathEvent, Action<int, float, float, bool> onHealthChangeEvent): base(maxHealth, onDeathEvent)
        {
            OnHealthChangeEvent = onHealthChangeEvent;
            HealthViewUpdate(0, false);
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            HealthViewUpdate(amount, true);
        }

        public override void Heal(float amount)
        {
            base.Heal(amount);
            HealthViewUpdate(amount, false);
        }

        private void HealthViewUpdate(float changeValue, bool IsDamageOrHeal) 
        {
            OnHealthChangeEvent?.Invoke((int)_currentHealth, _maxHealth, changeValue, IsDamageOrHeal);
        }
    }
}

