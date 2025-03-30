using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class FireTrailEffect
    {
        private readonly BulletData _bulletData;

        private readonly Rigidbody2D _rb;

        private float _lastSpawnTime;
        private float _lastSpawnTimer;

        private readonly IReadableModificator _damageModificator;
        private readonly IReadableModificator _criticalChanceModificator;
        private readonly IReadableModificator _criticalDamageMultiplier;
        private readonly IReadableModificator _bulletSize;

        private Transform _projectileParent;

        private ObjectPool<FireEffect> _pool;


        public FireTrailEffect(Rigidbody2D rb, BulletData data, IReadableModificator damageModificator,
            IReadableModificator criticalChangeModificator,
            IReadableModificator criticalDamageMultiplier,
            IReadableModificator bulletSize)
        {
            _rb = rb;
            _bulletData = data;
            _damageModificator = damageModificator;
            _criticalChanceModificator = criticalChangeModificator;
            _criticalDamageMultiplier = criticalDamageMultiplier;
            _bulletSize = bulletSize;
        }

        private void CreateFireParent()
        {
            _projectileParent = new GameObject($"[FireTrail]").transform;
            _projectileParent.position = Vector3.zero;
        }

        private FireEffect CreateFire()
        {
            GameObject fireInstance = GameObject.Instantiate(_bulletData.BulletObject, _rb.transform.position, Quaternion.identity);
            fireInstance.transform.SetParent(_projectileParent);

            return fireInstance.GetComponent<FireEffect>();
        }

        private void ReturnFireToPool(FireEffect fire)
        {
            _pool.Return(fire);
        }

        private void Update()
        {
            if (_rb.velocity.magnitude > 0.1f)
            {
                _lastSpawnTimer -= Time.deltaTime;
                if (_lastSpawnTimer <= 0)
                {
                    SpawnFire();
                    _lastSpawnTimer = _lastSpawnTime;
                }
            }
        }

        private void SpawnFire()
        {

        }
    }
}

