using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class DashView : MonoBehaviour
    {
        private int _fireTraceStartPreloadCount = 30;

        private IReadableModificator _damageModificator;
        private IReadableModificator _criticalChanceModificator;
        private IReadableModificator _criticalDamageMultiplier;
        private IReadableModificator _bulletSize;

        private Rigidbody2D _dashRb;

        private BulletData _data;

        private IProjectileFactory _projectileFactory;

        private float _lastSpawnTime;
        private float _lastSpawnTimer;

        private bool _isEvolve;

        private Vector3 _lastPosition;

        private Color _evolvedColor;

        private bool _isActive;

        public void Init(BulletData data, float spawnTraceTimer, Color evolvedColor,
            IReadableModificator damageModificator, 
            IReadableModificator criticalChanceModificator, 
            IReadableModificator criticalDamageMultiplier, 
            IReadableModificator bulletSize)
        {
            //_dashRb = gameObject.GetComponent<Rigidbody2D>();
            _evolvedColor = evolvedColor;
            _data = data;
            _lastSpawnTimer = _lastSpawnTime = spawnTraceTimer;
            _damageModificator = damageModificator;
            _criticalChanceModificator = criticalChanceModificator;
            _criticalDamageMultiplier = criticalDamageMultiplier;
            _bulletSize = bulletSize;
        }

        private void CreateProjectFactory(BulletData data) 
        {
            _projectileFactory = new ProjectileFactory(data, _fireTraceStartPreloadCount,
                () => Object.Instantiate(data.BulletObject).GetComponent<FireEffect>(),
                _damageModificator,
                _criticalChanceModificator,
                _criticalDamageMultiplier,
                _bulletSize
                );
        }

        private bool HasMoved()
        {
            bool moved = Vector3.Distance(transform.position, _lastPosition) > 0.01f;
            _lastPosition = transform.position;
            return moved;
        }

        private void FireTrace()
        {
            if (HasMoved())
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
            _projectileFactory.CreateProjectile(gameObject.transform.position, Vector2.zero);
        }

        private void SetNewEvolvedColor() 
        {
            gameObject.GetComponent<SpriteRenderer>().color = _evolvedColor;
            gameObject.transform.Find("Particle_DashTrail").gameObject.SetActive(false);
        }

        public void Evolve(BulletData traceData) 
        {
            _data = traceData;
            CreateProjectFactory(traceData);
            SetNewEvolvedColor();
            _isEvolve = true;
        }

        public void Tick() 
        {
            if (_isEvolve) 
            {
                _projectileFactory?.Tick();
                if (_isActive) 
                {
                    FireTrace();
                }
            }
        }

        public void Activete() 
        {
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Stop() 
        {
            _isActive = false;
            gameObject.SetActive(false);
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(CalculateDamage(), CalculateCriticalChance(), CalculateCriticalMultiplier());

                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 knockbackDirection = (enemy.transform.position - gameObject.transform.position).normalized;
                    rb.AddForce(knockbackDirection * 2f, ForceMode2D.Force);
                }
            }
        }
    }
}

