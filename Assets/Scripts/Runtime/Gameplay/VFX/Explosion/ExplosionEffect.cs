using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ExplosionEffect : Effect
    {
        private ParticleSystem _particleSystem;

        public override void Init(Action<IEffect> returnToPoolAction)
        {
            base.Init(returnToPoolAction);

            InitParticleSystem();
        }

        private void InitParticleSystem() 
        {
            _particleSystem = gameObject.GetComponent<ParticleSystem>();

            ParticleSystem.MainModule main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        public void StartEffect(Vector3 position)
        {
            SetPosition(position);
            _particleSystem.Emit(500);
        }

        private void SetPosition(Vector3 position) 
        {
            gameObject.transform.position = position;
        }

        private void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
            _returnToPoolAction?.Invoke(this);
        }
    }
}