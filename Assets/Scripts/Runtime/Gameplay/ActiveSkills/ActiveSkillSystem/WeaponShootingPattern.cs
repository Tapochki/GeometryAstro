using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class WeaponShootingPattern : MonoBehaviour
    {
        public Transform Origin { get => _origin; }
        public Transform Direction { get => _direction; }
        public ActiveSkillType Type { get => _type; }
        public int Id { get => _id; }

        [SerializeField]
        private Transform _origin;
        [SerializeField]
        private Transform _direction;
        [SerializeField]
        private ActiveSkillType _type;
        [SerializeField]
        private int _id;

        private void Awake()
        {
            if (_origin == null)
                _origin = transform;

            if (_direction == null && transform.childCount > 0)
                _direction = transform.GetChild(0);

            if (_type == default && transform.parent != null)
            {
                if (System.Enum.TryParse(transform.parent.name, out ActiveSkillType parsedType))
                    _type = parsedType;
            }
        }
    }
}

