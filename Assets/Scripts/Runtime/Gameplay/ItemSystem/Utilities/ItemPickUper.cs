using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ItemPickUper : MonoBehaviour
    {
        private IReadableModificator _itemRadiusModificator;
        private CircleCollider2D _collider;

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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ItemView itemView))
            {
                itemView.FirstPickUp(gameObject.transform);
                return;
            }
        }
    }
}

