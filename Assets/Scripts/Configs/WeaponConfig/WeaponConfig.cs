using System;
using System.Collections.Generic;
using TandC.Settings;
using UnityEngine;

namespace TandC.Data
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
        public BulletData bulletData;
    }

    [Serializable]
    public class BulletData
    {
        public int BulletSpeed;
        public float bulletLifeTime;
        public int bulletLife;
        public GameObject ButlletObject;
        public WeaponType type;
        public bool IsNotCustomise;
        public CustomisationType CustomisationType;
    }
}