using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class ExplosionEffect : MonoBehaviour, IEffect
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        private Action<ExplosionEffect> _returnToPoolAction;

        public void Init(Action<IEffect> returnToPoolAction)
        {
            _returnToPoolAction = returnToPoolAction;

            InitParticleSystem();
        }

        private void InitParticleSystem() 
        {
            ParticleSystem.MainModule main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        public void Show()
        {
            gameObject.SetActive(true);
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

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Dispose() 
        {
            _returnToPoolAction = null;
            Destroy(gameObject);
        }

        private void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
            _returnToPoolAction?.Invoke(this);
        }
    }
}