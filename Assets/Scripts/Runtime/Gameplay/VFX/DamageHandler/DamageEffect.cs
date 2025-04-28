using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX 
{
    public class DamageEffect : MonoBehaviour, IEffect, ITickable
    {
        private Action<IEffect> _returnToPoolAction;
        private TextMesh _textMesh;

        public void Init(Action<IEffect> returnToPoolAction)
        {
            _returnToPoolAction = returnToPoolAction;
            _textMesh = gameObject.GetComponent<TextMesh>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void StartEffect(Vector3 position, int damage, Color color)
        {
            SetPosition(position);
            SetText(damage, color);
        }

        private void SetPosition(Vector3 position)
        {
            gameObject.transform.position = position;
        }

        private void SetText(int damage, Color color) 
        {
            _textMesh.text = damage.ToString();
            _textMesh.color = color;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _returnToPoolAction = null;
            Destroy(gameObject);
        }

        private void AnimationEndEvent() 
        {
            _returnToPoolAction?.Invoke(this);
        }

        public void Tick()
        {
            
        }
    }
}

