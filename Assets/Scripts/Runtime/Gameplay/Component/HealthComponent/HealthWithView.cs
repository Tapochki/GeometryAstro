using System;

namespace TandC.GeometryAstro.Gameplay 
{
    public class HealthWithView : BaseHealthComponent
    {
        public Action<int, float> OnHealthChangeEvent { get; protected set; }

        public HealthWithView(float maxHealth, Action<bool> onDeathEvent, Action<int, float> onHealthChangeEvent): base(maxHealth, onDeathEvent)
        {
            OnHealthChangeEvent = onHealthChangeEvent;
            HealthViewUpdate();
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            HealthViewUpdate();
        }

        public override void Heal(float amount)
        {
            base.Heal(amount);
            HealthViewUpdate();
        }

        private void HealthViewUpdate() 
        {
            OnHealthChangeEvent?.Invoke((int)_currentHealth, _maxHealth);
        }
    }
}

