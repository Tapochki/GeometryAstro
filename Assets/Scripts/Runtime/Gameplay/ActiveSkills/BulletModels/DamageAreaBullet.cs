using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class DamageAreaBullet : BaseBullet
    {
        IReadableModificator _areaRadiusModificator;

        public DamageAreaBullet SetExplosiveDamageAreaBullet(IReadableModificator areaRadiusModificator)
        {
            _areaRadiusModificator = areaRadiusModificator;
            return this;
        }

        private DamageAreaEffect _damageAreaEffect;

        protected override void BulletHit()
        {
            CreateDamageArea();
        }

        protected void CreateDamageArea() 
        {
            Deactivate();
            _damageAreaEffect = new DamageAreaEffect(gameObject.transform.position, _areaRadiusModificator.Value, LayerMask.GetMask("Enemy"), _bulletData.baseDamage, _criticalChance, _criticalMultiplier, EffectEndHandler, _bulletData.bulletLife, 0.5f);
        }

        protected void EffectEndHandler() 
        {
            _damageAreaEffect.Dispose();
            _damageAreaEffect = null;
            BackToPool();
        }

        public override void Tick()
        {
            base.Tick();
            _damageAreaEffect?.Tick();
        }
    }
}


