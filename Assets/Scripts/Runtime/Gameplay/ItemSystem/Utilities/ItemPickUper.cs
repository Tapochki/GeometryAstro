using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ItemPickUper : MonoBehaviour
    {
        private IReadableModificator _itemRadiusModificator;
        private CircleCollider2D _collider;

        private IReadOnlyReactiveProperty<int> _ammoCount;
        private IReadOnlyReactiveProperty<int> _maxAmmoCount;

        public void SetModificator(IReadableModificator itemRadiusModificator) 
        {
            _itemRadiusModificator = itemRadiusModificator;
            _collider = gameObject.GetComponent<CircleCollider2D>();

            _collider.radius = _itemRadiusModificator.Value;
            _itemRadiusModificator.OnValueChanged += OnRadiusUpgradeHandler;
        }

        private void OnRadiusUpgradeHandler(float value) 
        {
            _collider.radius = _itemRadiusModificator.Value;
        }

        public void SetCanPickUpRocket(IReadOnlyReactiveProperty<int> ammoCount, IReadOnlyReactiveProperty<int> maxAmmoCount) 
        {
            _ammoCount = ammoCount;
            _maxAmmoCount = maxAmmoCount;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ItemView itemView))
            {
                if (itemView.IsModelRocketAmmo()) 
                {
                    if (_ammoCount.Value == _maxAmmoCount.Value)
                        return;
                }
                itemView.FirstPickUp(gameObject.transform);
                return;
            }
        }
    }
}

