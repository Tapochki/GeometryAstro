using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class FreezeComponent : IFreezeComponent
    {
        public bool IsFreeze { get; private set; }

        private float _freezeTimer;

        private readonly GameObject _freezeVFX;

        private readonly Rigidbody2D _rigidbody2D;

        public Action EndFreezeEvent;

        public FreezeComponent(GameObject freezeVFX, Rigidbody2D rigidbody2D)
        {
            IsFreeze = false;
            _freezeVFX = freezeVFX;
            _rigidbody2D = rigidbody2D;

            SetFreezeObject(IsFreeze);
        }

        public void Tick()
        {
            _freezeTimer -= Time.deltaTime;
            if (_freezeTimer <= 0)
            {
                EndFreeze();
            }
        }

        public void Freeze(float freezeTimer)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            _freezeTimer = freezeTimer;
            IsFreeze = true;
            SetFreezeObject(IsFreeze);
        }

        private void EndFreeze()
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            IsFreeze = false;
            SetFreezeObject(IsFreeze);
            EndFreezeEvent.Invoke();
        }

        private void SetFreezeObject(bool isActive)
        {
            _freezeVFX.gameObject.SetActive(isActive);
        }
    }
}
