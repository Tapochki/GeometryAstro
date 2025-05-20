using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX 
{
    public class EnemyDeathEffect : Effect
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        public override void Init(Action<IEffect> returnToPoolAction)
        {
            base.Init(returnToPoolAction);

            InitParticleSystem();
        }

        private void InitParticleSystem()
        {
            ParticleSystem.MainModule main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        public void StartEffect(Vector3 position)
        {
            SetPosition(position);
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

