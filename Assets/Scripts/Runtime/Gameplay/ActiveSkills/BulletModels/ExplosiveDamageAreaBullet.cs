
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ExplosiveDamageAreaBullet : BaseBullet
    {
        private DamageAreaEffect _damageAreaEffect;

        IReadableModificator _areaRadiusModificator;

        public ExplosiveDamageAreaBullet SetExplosiveDamageAreaBullet(IReadableModificator areaRadiusModificator)
        {
            _areaRadiusModificator = areaRadiusModificator;
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
            float damageAreaRadius = _areaRadiusModificator.Value; //TODO Take Radius From Data
            float duration = _bulletData.bulletLife;
            float tickInterval = 0.5f; // standart damage interval for zone
            float damagePerTick = _bulletData.baseDamage / 10;

            Deactivate();
            _damageAreaEffect = new DamageAreaEffect(transform.position, damageAreaRadius, LayerMask.GetMask("Enemy"), damagePerTick, _criticalChance, _criticalMultiplier, EffectEndHandler, duration, tickInterval);
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
