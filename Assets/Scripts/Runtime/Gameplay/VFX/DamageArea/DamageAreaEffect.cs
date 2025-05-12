using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class DamageAreaEffect : MonoBehaviour, IEffect
    {
        private Action<DamageAreaEffect> _returnToPoolAction;

        private float _timer;
        private bool _isStart;

        public void Init(Action<IEffect> returnToPoolAction)
        {
            _returnToPoolAction = returnToPoolAction;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void StartEffect(Vector3 position, float timer)
        {
            SetPosition(position);
            _timer = timer;
            _isStart = true;
        }

        private void SetPosition(Vector3 position) 
        {
            gameObject.transform.position = position;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Update()
        {
            if (_isStart) 
            {
                _timer -= Time.deltaTime;
                if(_timer <= 0) 
                {
                    EndEffect();
                }
            }
        }

        private void EndEffect() 
        {
            _isStart = false;
            _returnToPoolAction.Invoke(this);
        }

        public void Dispose() 
        {
            _returnToPoolAction = null;
            Destroy(gameObject);
        }
    }
}