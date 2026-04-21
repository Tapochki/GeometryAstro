using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// MonoBehaviour placed on weapon prefab child objects.
    /// Origin = bullet spawn point.
    /// Direction = point the bullet flies toward (aim reference).
    /// Id = determines which patterns are active at each level (0–4).
    /// </summary>
    public class WeaponShootingPattern : MonoBehaviour
    {
        [SerializeField] private Transform _origin;
        [SerializeField] private Transform _direction;
        [SerializeField] private int _id;

        public Transform Origin => _origin;
        public Transform Direction => _direction;
        public int Id => _id;

        private void Awake()
        {
            if (_origin == null)
                _origin = transform;

            if (_direction == null && transform.childCount > 0)
                _direction = transform.GetChild(0);
        }
    }
}
