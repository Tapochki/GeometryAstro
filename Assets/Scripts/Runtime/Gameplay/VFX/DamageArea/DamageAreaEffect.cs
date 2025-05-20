using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class DamageAreaEffect : Effect
    {
        private float _timer;
        private bool _isStart;

        public override void Init(Action<IEffect> returnToPoolAction)
        {
           base.Init(returnToPoolAction);
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
    }
}