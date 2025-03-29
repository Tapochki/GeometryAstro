
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ExplosiveDamageAreaBullet : BaseBullet
    {
        private DamageAreaEffect _damageAreaEffect;

        IReadableModificator _areaRadiusModificator;

        private float _damageInterval;

        public ExplosiveDamageAreaBullet SetExplosiveDamageAreaBullet(IReadableModificator areaRadiusModificator, float damageInterval)
        {
            _areaRadiusModificator = areaRadiusModificator;
            _damageInterval = damageInterval;
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
            new ExplosionDamage().ApplyExplosionDamage(transform.position, explosionRadius, _bulletData.baseDamage, _criticalChance, _criticalMultiplier);
        }

        private void CreateDamageArea()
        {
            float damageAreaRadius = _areaRadiusModificator.Value;
            float duration = _bulletData.bulletLife;
            float damagePerTick = _bulletData.baseDamage / 10;

            Deactivate();
            _damageAreaEffect = new DamageAreaEffect(transform.position, damageAreaRadius, 
                LayerMask.GetMask("Enemy"), damagePerTick, _criticalChance, _criticalMultiplier, EffectEndHandler, duration, _damageInterval);
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
