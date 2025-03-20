using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class HealthRegenerator
    {
        private IHealth _health;
        private float _regenAmount;
        private float _regenCooldown;
        private float _timeSinceLastRegen;

        public HealthRegenerator(IHealth health, float regenAmount, float regenCooldown)
        {
            _health = health;
            _regenAmount = regenAmount;
            _regenCooldown = regenCooldown;
        }

        public void Tick()
        {
            if (_health.CurrentHealth > 0)
            {
                _timeSinceLastRegen += Time.deltaTime;
                if (_timeSinceLastRegen >= _regenCooldown)
                {
                    _health.Heal(_regenAmount);
                    _timeSinceLastRegen = 0;
                }
            }
        }
    }
}

