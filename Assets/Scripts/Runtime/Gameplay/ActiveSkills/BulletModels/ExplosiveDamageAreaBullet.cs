
using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ExplosiveDamageAreaBullet : BaseBullet
    {
        private DamageAreaEffect _damageAreaEffect;
        private ExplosionDamage _explosionDamage;

        IReadableModificator _areaRadiusModificator;

        private float _damageInterval;

        public ExplosiveDamageAreaBullet SetExplosiveDamageAreaBullet(IReadableModificator areaRadiusModificator, float damageInterval)
        {
            _areaRadiusModificator = areaRadiusModificator;
            _damageInterval = damageInterval;
            _explosionDamage = new ExplosionDamage();
            return this;
        }

        protected override void BulletHit()
        {
            CreateExplosion();
            CreateDamageArea();
        }

        private void CreateExplosion()
        {
            float explosionRadius = _areaRadiusModificator.Value;
            _explosionDamage.ApplyExplosionDamage(transform.position, explosionRadius, _bulletData.baseDamage, _criticalChance, _criticalMultiplier);
            EventBusHolder.EventBus.Raise(new CreateExplosionEffect(gameObject.transform.position, _areaRadiusModificator.Value));

        }

        private void CreateDamageArea()
        {
            float damageAreaRadius = _areaRadiusModificator.Value;
            float duration = _bulletData.bulletLife;
            float damagePerTick = _bulletData.baseDamage / 10;

            Deactivate();
            _damageAreaEffect = new DamageAreaEffect(transform.position, damageAreaRadius, 
                LayerMask.GetMask("Enemy"), damagePerTick, _criticalChance, _criticalMultiplier, EffectEndHandler, duration, _damageInterval);
            EventBusHolder.EventBus.Raise(new CreateDamageAreaEffect(gameObject.transform.position, _areaRadiusModificator.Value, duration));
        }

        private void EffectEndHandler()
        {
            _damageAreaEffect.Dispose();
            _damageAreaEffect = null;
        }

        public override void Tick()
        {
            base.Tick();
            _damageAreaEffect?.Tick();
        }
    }
}
