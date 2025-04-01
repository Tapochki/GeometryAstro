using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ExplosionEffect : MonoBehaviour
    {
        public ParticleSystem particleSystem;

        private Action<ExplosionEffect> _returnToPoolAction;
        
        public void Init(Action<ExplosionEffect> returnToPoolAction)
        {
            var main = particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
            _returnToPoolAction = returnToPoolAction;
            particleSystem.Emit(500);
        }
        
        private void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
            _returnToPoolAction?.Invoke(this);
            _returnToPoolAction = null;
        }
    }
}