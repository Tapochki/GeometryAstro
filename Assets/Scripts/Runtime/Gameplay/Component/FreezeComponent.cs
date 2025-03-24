using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class FreezeComponent : IFreezeComponent
    {
        public bool IsFreeze { get; private set; }

        private float _freezeTimer;

        private readonly GameObject _freezeVFX;

        public FreezeComponent(GameObject freezeVFX)
        {
            IsFreeze = false;
            _freezeVFX = freezeVFX;

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
            _freezeTimer = freezeTimer;
            IsFreeze = true;
            SetFreezeObject(IsFreeze);
        }

        private void EndFreeze()
        {
            IsFreeze = false;
            SetFreezeObject(IsFreeze);
        }

        private void SetFreezeObject(bool isActive)
        {
            _freezeVFX.gameObject.SetActive(isActive);
        }
    }
}
