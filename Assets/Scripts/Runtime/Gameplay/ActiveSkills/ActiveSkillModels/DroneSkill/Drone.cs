using DG.Tweening;
using System;
using TandC.GeometryAstro.Data;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay 
{
    public class Drone : MonoBehaviour, ITickable
    {
        private IReadableModificator _damageModificator;
        private IReadableModificator _criticalChanceModificator;
        private IReadableModificator _criticalDamageMultiplier;

        private Action<Drone> _backToPoolEvent;
    
        private SpriteRenderer _spriteRenderer;
        private Tween _fadeTween;

        private BulletData _data;

        private float _lifeTimer;

        private float _fadeDuration = 0.25f;
        private bool _isEvolved;
        private bool _isStartFade;

        public bool IsOld { get; private set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Init(BulletData data,
            IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplier,
            Action<Drone> backToPoolEvent
            )
        {
            _data = data;
            _damageModificator = damageModificator;
            _criticalChanceModificator = criticalChanceModificator;
            _criticalDamageMultiplier = criticalDamageMultiplier;
            _backToPoolEvent = backToPoolEvent;
        }

        public void SetEvolve(bool isEvolved) 
        {
            _isEvolved = isEvolved;
        }

        public void SetPosition(Vector3 position) 
        {
            gameObject.transform.position = position;
        }

        public void SetOldDroneOld()
        {
            IsOld = true;
        }

        public void Tick() 
        {
            if (_isEvolved) 
            {
                return;
            }
            if (!_isStartFade) 
            {
                _lifeTimer -= Time.deltaTime;
                if (_lifeTimer <= 0)
                {
                    FadeOut();
                }
            }
        }

        public void Show()
        {
            _fadeTween?.Kill();
            _isStartFade = false;
            _lifeTimer = _data.bulletLifeTime;
            gameObject.SetActive(true);

            FadeIn();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void FadeIn() 
        {
            _fadeTween?.Kill();

            SetAlpha(0f);

            _fadeTween = _spriteRenderer
                .DOFade(1f, _fadeDuration);
        }

        public void ForcedFadeOut() 
        {
            FadeOut();
        }

        private void FadeOut() 
        {
            _isStartFade = true;
            _fadeTween?.Kill();

            DOTween.To(() => _spriteRenderer.color.a,
                x => _spriteRenderer.color = new Color(1, 1, 1, x),
                0f,
                _fadeDuration)
                .OnComplete(EndFadeOut);
        }

        private void EndFadeOut() 
        {
            Debug.LogError("EndFadeOut");
            _backToPoolEvent?.Invoke(this);
        }

        private void SetAlpha(float value)
        {
            _spriteRenderer.color = new Color(
                _spriteRenderer.color.r,
                _spriteRenderer.color.g,
                _spriteRenderer.color.b,
                value
            );
        }

        private void HitEnemy(Enemy enemy) 
        {
            enemy.TakeDamage(CalculateDamage(), CalculateCriticalChance(), CalculateCriticalMultiplier());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                HitEnemy(enemy);
            }
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

        public void Dispose()
        {
            _backToPoolEvent = null;
            _fadeTween?.Kill();
            Destroy(gameObject);
        }
    }
}

