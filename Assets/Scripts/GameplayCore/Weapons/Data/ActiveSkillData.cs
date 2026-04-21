using UnityEngine;

namespace GameplayCore
{
    [System.Serializable]
    public class ActiveSkillData
    {
        [Tooltip("Prefab with WeaponShootingPattern components.")]
        public GameObject SkillPrefab;

        public BulletData BulletData;

        [Tooltip("Upgraded bullet data used after Evolve() is called.")]
        public BulletData EvolvedBulletData;

        [Tooltip("Seconds between shots.")]
        public float ShootDelay = 0.5f;

        [Tooltip("How far the raycast checks for enemies.")]
        public float DetectorRadius = 20f;

        [Tooltip("Bullet spread half-range used by MachineGun / RifleGun.")]
        public float SpreadRadius = 2f;
    }
}
