using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class FlashSpriteComponent : ITickable
    {
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Color _originColor;
        private readonly Color _flashColor = Color.red;

        private float _flashTimer;

        private float _flashTime = 0.1f;

        private bool _isFlashing;

        public FlashSpriteComponent(SpriteRenderer spriteRenderer, Color originColor) 
        {
            _spriteRenderer = spriteRenderer;
            _originColor = originColor;
        }

        public void Tick()
        {
            if (_isFlashing) 
            {
                _flashTimer-= Time.deltaTime;
                if(_flashTimer <= 0 ) 
                {
                    SetSpriteColor(_originColor);
                }
            }
        }

        private void SetSpriteColor(Color color ) 
        {
            _spriteRenderer.color = color;
        }

        public void StartFlash() 
        {
            _flashTimer = 0.1f;
            SetSpriteColor(_flashColor);
            _isFlashing = true;
        }
    }
}

