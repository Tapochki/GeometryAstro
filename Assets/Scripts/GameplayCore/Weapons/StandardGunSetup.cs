using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Drop this onto the Player GameObject alongside Player.cs.
    /// Assign a StandardGunConfig ScriptableObject and the gun will be
    /// created and registered automatically on Awake.
    ///
    /// Setup checklist:
    ///   1. Create a StandardGunConfig asset (Assets > Create > GameplayCore > Standard Gun Config).
    ///   2. Fill in BulletData (BulletObject prefab, speed, damage, etc.).
    ///   3. Create a skill prefab with one or more WeaponShootingPattern components (Id 0–4).
    ///   4. Assign prefab to StandardGunConfig.Data.SkillPrefab.
    ///   5. Assign your SkillMount transform (child of Player) or leave empty to use Player root.
    ///   6. Make sure enemy GameObjects implement IDamageable and are on the "Enemy" layer.
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class StandardGunSetup : MonoBehaviour
    {
        [SerializeField] private StandardGunConfig _config;

        [Tooltip("Parent transform for the weapon pattern prefab. Defaults to Player root.")]
        [SerializeField] private Transform _skillMount;

        private void Awake()
        {
            if (_config == null)
            {
                Debug.LogError("[StandardGunSetup] No StandardGunConfig assigned!", this);
                return;
            }

            Player player = GetComponent<Player>();
            StandardGun gun = BuildGun();
            player.RegisterWeapon(gun);
        }

        private StandardGun BuildGun()
        {
            ActiveSkillData data = _config.Data;
            Transform mount = _skillMount != null ? _skillMount : transform;

            var gun = new StandardGun();
            gun.SetData(data);

            gun.SetProjectileFactory(new ProjectileFactory(
                data.BulletData,
                preloadCount: 10,
                bulletCreator: () => Instantiate(data.BulletData.BulletObject).GetComponent<BaseBullet>()));

            gun.SetReloader(new SkillReloader(data.ShootDelay));

            gun.SetEnemyDetector(new RaycastEnemyDetector(LayerMask.GetMask("Enemy")));

            gun.RegisterDuplicator(extraDuplicates: 0);

            gun.RegisterShootingPatterns(mount);

            return gun;
        }
    }
}
