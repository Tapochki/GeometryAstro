using System;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// A single orbiting drone. Damages IDamageable targets on trigger contact.
    /// Lifetime is controlled by BulletData.BulletLifeTime.
    ///
    /// Note: Add a SpriteRenderer and Collider2D (set as Trigger) to the prefab.
    /// Drones must be on a layer that collides with the enemy layer.
    ///
    /// DOTween fade animations from the original have been removed.
    /// Re-add them here if DOTween is available in your target project.
    /// </summary>
    public class Drone : MonoBehaviour, ITickable
    {
        private BulletData _data;
        private float _damage;
        private float _critChance;
        private float _critMultiplier;

        private Action<Drone> _returnToPoolCallback;

        private float _lifeTimer;
        private bool _isEvolved;

        public bool IsOld { get; private set; }

        public void Init(
            BulletData data,
            float damage,
            float critChance,
            float critMultiplier,
            Action<Drone> returnToPoolCallback)
        {
            _data = data;
            _damage = damage;
            _critChance = critChance;
            _critMultiplier = critMultiplier;
            _returnToPoolCallback = returnToPoolCallback;
        }

        public void SetEvolved(bool isEvolved) => _isEvolved = isEvolved;

        public void SetPosition(Vector3 position) => transform.position = position;

        public void SetOld() => IsOld = true;

        public void Show()
        {
            _lifeTimer = _data.BulletLifeTime;
            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);

        /// <summary>Call this to force the drone back to pool immediately.</summary>
        public void ForceReturn() => EndLife();

        public void Tick()
        {
            if (_isEvolved) return;

            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer <= 0f)
                EndLife();
        }

        private void EndLife() => _returnToPoolCallback?.Invoke(this);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamageable target))
                target.TakeDamage(_damage, _critChance, _critMultiplier);
        }

        public void Dispose()
        {
            _returnToPoolCallback = null;
            Destroy(gameObject);
        }
    }
}
