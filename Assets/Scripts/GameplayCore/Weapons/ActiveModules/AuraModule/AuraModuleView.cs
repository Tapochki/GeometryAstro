using System.Collections.Generic;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// MonoBehaviour placed on the aura prefab's collider object.
    /// Tracks enemies inside the trigger zone and applies damage on demand.
    ///
    /// Requirements:
    ///   - The GameObject must have a Trigger Collider2D.
    ///   - Enemy GameObjects must implement IDamageable and be on the "Enemy" layer.
    /// </summary>
    public class AuraModuleView : MonoBehaviour
    {
        private readonly List<IDamageable> _enemiesInZone = new();

        private BulletData _data;
        private float _damage;
        private float _critChance;
        private float _critMultiplier;
        private bool _isEvolved;

        // ── Setup ─────────────────────────────────────────────────────────────────

        public void Init(BulletData data, float damage, float critChance, float critMultiplier)
        {
            _data = data;
            _damage = damage;
            _critChance = critChance;
            _critMultiplier = critMultiplier;
        }

        public void SetActive(bool value) => gameObject.SetActive(value);

        public void Upgrade(float sizeIncrease)
        {
            Vector3 s = transform.localScale;
            transform.localScale = new Vector3(s.x + sizeIncrease, s.y + sizeIncrease, s.z);
        }

        public void Evolve(BulletData evolvedData)
        {
            _data = evolvedData;
            _damage = evolvedData.BaseDamage;
            _critChance = evolvedData.BasicCriticalChance;
            _critMultiplier = evolvedData.BasicCriticalMultiplier;
            _isEvolved = true;

            if (TryGetComponent(out SpriteRenderer sr))
                sr.color = Color.red;
        }

        // ── Damage application ────────────────────────────────────────────────────

        public void ApplyDamage()
        {
            for (int i = _enemiesInZone.Count - 1; i >= 0; i--)
            {
                if (_enemiesInZone[i] == null)
                {
                    _enemiesInZone.RemoveAt(i);
                    continue;
                }

                _enemiesInZone[i].TakeDamage(_damage, _critChance, _critMultiplier);
            }
        }

        // ── Trigger tracking ──────────────────────────────────────────────────────

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                _enemiesInZone.Add(damageable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                _enemiesInZone.Remove(damageable);
        }
    }
}
