using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "TandC/Game/WeaponConfig", order = 1)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private List<WeaponData> _weaponData;

        public WeaponData GetWeaponByType(WeaponType weaponType)
        {
            foreach (var weapon in _weaponData)
            {
                if (weapon.type == weaponType)
                {
                    return weapon;
                }
            }

            return null;
        }

    }
    [Serializable]
    public class WeaponData
    {
        public WeaponType type;
        public string weaponName;
        public float baseDamage;
        public float shootDeley;
        public float detectorDistance;
        public BulletData bulletData;
    }

    [Serializable]
    public class BulletData
    {
        public int BulletSpeed;
        public float bulletLifeTime;
        public int bulletLife;
        public GameObject BulletObject;
        public WeaponType type;
        public bool IsNotCustomise;
        public CustomisationType CustomisationType;
    }
}