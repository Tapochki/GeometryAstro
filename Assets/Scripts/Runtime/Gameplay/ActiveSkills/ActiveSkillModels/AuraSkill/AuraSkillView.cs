using System.Collections.Generic;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class AuraSkillView : MonoBehaviour
    {
        private List<Enemy> _enemiesInZone;
        private Collider2D _collider;

        private bool _isEvolved;

        private IReadableModificator _damageModificator;
        private IReadableModificator _criticalChanceModificator;
        private IReadableModificator _criticalDamageMultiplier;
        private IReadableModificator _bulletSize;

        private BulletData _data;

        public void Init(BulletData data,
            IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplier,
            IReadableModificator bulletSize)
        {
            _data = data;
            _damageModificator = damageModificator;
            _criticalChanceModificator = criticalChanceModificator;
            _criticalDamageMultiplier = criticalDamageMultiplier;
            _bulletSize = bulletSize;

            _isEvolved = false;
            _enemiesInZone = new List<Enemy>();
            _collider = transform.GetComponent<Collider2D>();
            _collider.isTrigger = true;
        }

        public void Upgrade(float value) 
        {
            Vector3 scale = _collider.transform.localScale;
            _collider.transform.localScale = new Vector3(scale.x + value, scale.y + value);
        }

        public void Evolve(BulletData data) 
        {
            _data = data;
            _isEvolved = true;
            _collider.GetComponent<SpriteRenderer>().color = Color.red;
        }

        public void SetActive(bool value) 
        {
            gameObject.SetActive(value);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                _enemiesInZone.Add(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy)) 
            {
                _enemiesInZone.Remove(enemy);
            }
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

        public void ApplyDamage()
        {
            for (int i = _enemiesInZone.Count - 1; i >= 0; i--)
            {
                if (_enemiesInZone[i] == null || !_enemiesInZone[i].IsActive)
                {
                    _enemiesInZone.RemoveAt(i);
                    continue;
                }

                _enemiesInZone[i].TakeDamage(CalculateDamage(), CalculateCriticalChance(), CalculateCriticalMultiplier());
            }

            if (_isEvolved) 
            {
                EventBusHolder.EventBus.Raise(new PlayerHealReleaseEvent(_enemiesInZone.Count / 2));
            }
        }
    }
}

