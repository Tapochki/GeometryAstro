using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class HealthRegenerator
    {
        private IHealth _health;
        private IReadableModificator _regenModificator;
        private float _timeSinceLastRegen;

        public HealthRegenerator(IHealth health, IReadableModificator regenModificator)
        {
            _health = health;
            _regenModificator = regenModificator;
            _timeSinceLastRegen = 1f;
        }

        public void Tick()
        {
            if(_regenModificator.Value > 0) 
            {
                if (_health.CurrentHealth > 0)
                {
                    _timeSinceLastRegen -= Time.deltaTime;
                    if (_timeSinceLastRegen <= 0)
                    {
                        _health.Heal(_regenModificator.Value);
                        _timeSinceLastRegen = 1f;
                    }
                }
            }
        }
    }
}

