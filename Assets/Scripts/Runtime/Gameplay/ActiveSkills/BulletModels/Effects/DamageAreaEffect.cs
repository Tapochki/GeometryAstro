using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class DamageAreaEffect
    {
        private readonly float _damageInterval = 0.5f;
        private readonly float _duration = 5f;
        private readonly float _radius;

        private readonly LayerMask _enemyLayer;
        private float _timer;
        private float _nextDamageTime;

        private Action _callBack;

        private Vector2 _position;
        private float _damage;
        private float _critChance;
        private float _critMultiplier;

        private bool _isEffectStarted;

        private DamageZoneVFX _vfx;

        public DamageAreaEffect(Vector2 position, float radius, LayerMask enemyLayer, float damage, float critChance, float critMultiplier, Action callBack, float duration = 5f, float damageInterval = 0.5f)
        {
            _callBack = callBack;
            _position = position;
            _radius = radius;
            _enemyLayer = enemyLayer;
            _timer = _duration;
            _damageInterval = damageInterval;
            _duration = duration;
            _nextDamageTime = _damageInterval;
            _damage = damage;
            _critChance = critChance;
            _critMultiplier = critMultiplier;

            _vfx = new DamageZoneVFX(position, radius, duration); //change to normal VFX

            _isEffectStarted = true;

            ApplyDamage();
        }

        public void Tick()
        {
            if (!_isEffectStarted)
                return;

            if(_timer <= 0) 
            {
                EffectEnd();
                return;
            }

            if (Time.time >= _nextDamageTime)
            {
                _nextDamageTime = Time.time + _damageInterval;
                ApplyDamage();
            }

            _timer -= Time.deltaTime;
        }

        private void EffectEnd() 
        {

            _callBack.Invoke();
        }

        public void Dispose() 
        {
            _callBack = null;
        }

        private void ApplyDamage()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(_position, _radius, _enemyLayer);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(_damage, _critChance, _critMultiplier);
                }
            }
        }

        public class DamageZoneVFX
        {
            private GameObject _zone;

            public DamageZoneVFX(Vector2 position, float radius, float duration)
            {
                _zone = new GameObject("DamageZoneVFX");
                _zone.transform.position = position;

                SpriteRenderer renderer = _zone.AddComponent<SpriteRenderer>();
                renderer.sprite = CreateCircleSprite();
                renderer.color = new Color(1f, 0f, 0f, 0.3f);

                _zone.transform.localScale = Vector3.one * radius * 2;

                UnityEngine.Object.Destroy(_zone, duration);
            }

            private static Sprite CreateCircleSprite()
            {
                Texture2D texture = new Texture2D(128, 128);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                return sprite;
            }
        }
    }
}

