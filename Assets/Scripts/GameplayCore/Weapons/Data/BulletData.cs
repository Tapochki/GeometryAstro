using UnityEngine;

namespace GameplayCore
{
    [System.Serializable]
    public class BulletData
    {
        [Tooltip("Prefab with BaseBullet component.")]
        public GameObject BulletObject;

        [Tooltip("Units per second.")]
        public float BulletSpeed = 15f;

        [Tooltip("Seconds until the bullet disappears.")]
        public float BulletLifeTime = 3f;

        [Tooltip("Number of hits before the bullet is returned to pool.")]
        public float BulletLife = 1f;

        public float BaseDamage = 10f;
        public float BasicCriticalChance = 0f;
        public float BasicCriticalMultiplier = 1.5f;
        public float BasicBulletSize = 1f;
    }
}
