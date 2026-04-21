using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Drop on the Player GameObject. Creates and registers an AutomaticGun.
    /// Needs a "Enemy" layer for detection.
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class AutomaticGunSetup : MonoBehaviour
    {
        [SerializeField] private AutomaticGunConfig _config;
        [SerializeField] private Transform _skillMount;

        private void Awake()
        {
            if (_config == null) { Debug.LogError("[AutomaticGunSetup] Config missing!", this); return; }

            ActiveSkillData d = _config.Data;
            Transform mount = _skillMount != null ? _skillMount : transform;

            var gun = new AutomaticGun();
            gun.SetData(d);
            gun.SetProjectileFactory(new ProjectileFactory(d.BulletData, 10,
                () => Instantiate(d.BulletData.BulletObject).GetComponent<BaseBullet>()));
            gun.SetReloader(new SkillReloader(d.ShootDelay));
            gun.SetEnemyDetector(new CircleFirstEnemyDetector(LayerMask.GetMask("Enemy")));
            gun.RegisterShootingPatterns(mount);

            GetComponent<Player>().RegisterWeapon(gun);
        }
    }

    /// <summary>
    /// Drop on the Player GameObject. Creates and registers a MachineGun.
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class MachineGunSetup : MonoBehaviour
    {
        [SerializeField] private MachineGunConfig _config;
        [SerializeField] private Transform _skillMount;

        private void Awake()
        {
            if (_config == null) { Debug.LogError("[MachineGunSetup] Config missing!", this); return; }

            ActiveSkillData d = _config.Data;
            Transform mount = _skillMount != null ? _skillMount : transform;

            var gun = new MachineGun();
            gun.SetData(d);
            gun.SetProjectileFactory(new ProjectileFactory(d.BulletData, 20,
                () => Instantiate(d.BulletData.BulletObject).GetComponent<BaseBullet>()));
            gun.SetReloader(new SkillReloader(d.ShootDelay));
            gun.SetStartShotsPerCycle(_config.StartShotsPerCycle);
            gun.RegisterShootingPatterns(mount);

            GetComponent<Player>().RegisterWeapon(gun);
        }
    }

    /// <summary>
    /// Drop on the Player GameObject. Creates and registers a RifleGun.
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class RifleGunSetup : MonoBehaviour
    {
        [SerializeField] private RifleGunConfig _config;
        [SerializeField] private Transform _skillMount;

        private void Awake()
        {
            if (_config == null) { Debug.LogError("[RifleGunSetup] Config missing!", this); return; }

            ActiveSkillData d = _config.Data;
            Transform mount = _skillMount != null ? _skillMount : transform;

            var gun = new RifleGun();
            gun.SetData(d);
            gun.SetProjectileFactory(new ProjectileFactory(d.BulletData, 20,
                () => Instantiate(d.BulletData.BulletObject).GetComponent<BaseBullet>()));
            gun.SetReloader(new SkillReloader(d.ShootDelay));
            gun.SetStartShotsPerCycle(_config.StartShotCount);
            gun.RegisterShootingPatterns(mount);

            GetComponent<Player>().RegisterWeapon(gun);
        }
    }

    /// <summary>
    /// Drop on the Player GameObject. Creates and registers a DroneModule.
    /// The drone prefab (BulletData.BulletObject) must have a Drone component,
    /// SpriteRenderer, and a Trigger Collider2D.
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class DroneModuleSetup : MonoBehaviour
    {
        [SerializeField] private DroneModuleConfig _config;
        [SerializeField] private Transform _skillMount;

        private void Awake()
        {
            if (_config == null) { Debug.LogError("[DroneModuleSetup] Config missing!", this); return; }

            ActiveSkillData d = _config.Data;
            Transform mount = _skillMount != null ? _skillMount : transform;

            var drone = new DroneModule();
            drone.SetData(d);
            drone.SetReloader(new SkillReloader(d.ShootDelay));
            drone.SetObject(mount, transform, _config.Damage, _config.CritChance, _config.CritMultiplier);

            GetComponent<Player>().RegisterWeapon(drone);
        }
    }
}
