using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ExplosiveBullet : BaseBullet
    {
        private IReadableModificator _areaRadiusModificator;

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
            EventBusHolder.EventBus.Raise(new CreateExplosion(gameObject.transform.position, _areaRadiusModificator.Value));
            //change hardcode radius to data.radius
            Create(gameObject.transform.position, _areaRadiusModificator.Value);
            new ExplosionDamage().ApplyExplosionDamage(gameObject.transform.position, _areaRadiusModificator.Value, _bulletData.baseDamage, _criticalChance, _criticalMultiplier);
        }

        //Test change to VfX
        private void Create(Vector2 position, float radius, float duration = 0.3f)
        {
            Debug.LogError("Stop");
            GameObject explosion = new GameObject("ExplosionVFX");
            explosion.transform.position = position;

            SpriteRenderer renderer = explosion.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateCircleSprite();
            renderer.color = new Color(1f, 0.5f, 0f, 0.5f);

            explosion.transform.localScale = Vector3.one * radius * 1.5f;

            Object.Destroy(explosion, duration);
        }

        private Sprite CreateCircleSprite()
        {
            int size = 128;
            Texture2D texture = new Texture2D(size, size);
            Color transparent = new Color(0, 0, 0, 0);
            Color circleColor = new Color(1f, 0.5f, 0f, 0.5f);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float dx = x - size / 2;
                    float dy = y - size / 2;
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);

                    if (distance < size / 2)
                        texture.SetPixel(x, y, circleColor);
                    else
                        texture.SetPixel(x, y, transparent);
                }
            }

            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }
    }
}


