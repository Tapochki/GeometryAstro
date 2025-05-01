using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX 
{
    public class EnemyDeathEffect : MonoBehaviour, IEffect, ITickable
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        private Action<IEffect> _returnToPoolAction;

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

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _returnToPoolAction = null;
            Destroy(gameObject);
        }

        public void StartEffect(Vector3 position)
        {
            SetPosition(position);
        }

        private void SetPosition(Vector3 position)
        {
            gameObject.transform.position = position;
        }

        public void Tick()
        {
            
        }

        private void OnDestroy()
        {
            Debug.LogError(12);
        }

        private void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
            _returnToPoolAction?.Invoke(this);
        }
    }
}

