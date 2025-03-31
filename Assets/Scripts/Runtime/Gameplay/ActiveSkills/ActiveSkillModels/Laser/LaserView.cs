using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class LaserView : MonoBehaviour
    {
        private BulletData _data;

        private IReadableModificator _damageModificator;
        private IReadableModificator _criticalChanceModificator;
        private IReadableModificator _criticalDamageMultiplier;
        private IReadableModificator _bulletSize;

        public void Init(BulletData data,
            IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplier
            ) 
        {
            _data = data;
            _damageModificator = damageModificator;
            _criticalChanceModificator = criticalChanceModificator;
            _criticalDamageMultiplier = criticalDamageMultiplier;
        }

        public void Evolve(BulletData data) 
        {
            _data = data;
        }

        private float CalculateCriticalChance()
        {
            return _data.BasicCriticalChance + _criticalChanceModificator.Value;
        }

        private float CalculateCriticalMultiplier()
        {
            return _data.BasicCriticalMultiplier + _criticalDamageMultiplier.Value;
        }

        private float CalculateDamage()
        {
            return _data.baseDamage * _damageModificator.Value;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(CalculateDamage(), CalculateCriticalChance(), CalculateCriticalMultiplier());
            }
        }
    }
}

