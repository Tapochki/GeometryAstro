using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ExplosiveBullet : BaseBullet
    {
        IReadableModificator _areaRadiusModificator;

        public ExplosiveBullet SetExplosiveDamageAreaBullet(IReadableModificator areaRadiusModificator)
        {
            _areaRadiusModificator = areaRadiusModificator;
            return this;
        }

        protected override void BulletHit()
        {
            CreateExplosion();
            BackToPool();
        }

        protected void CreateExplosion() 
        {
            //change hardcode radius to data.radius
            Create(gameObject.transform.position, _areaRadiusModificator.Value);
            new ExplosionDamage().ApplyExplosionDamage(gameObject.transform.position, _areaRadiusModificator.Value, _bulletData.baseDamage, _criticalChance, _criticalMultiplier);
        }

        //Test change to VfX
        private void Create(Vector2 position, float radius, float duration = 0.3f)
        {
            GameObject explosion = new GameObject("ExplosionVFX");
            explosion.transform.position = position;

            SpriteRenderer renderer = explosion.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateCircleSprite();
            renderer.color = new Color(1f, 0.5f, 0f, 0.5f);

            explosion.transform.localScale = Vector3.one * radius * 2f;

            Object.Destroy(explosion, duration);
        }

        private Sprite CreateCircleSprite()
        {
            Texture2D texture = new Texture2D(128, 128);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            return sprite;
        }
    }
}


