using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Static factory. Creates a fully wired WeaponModuleViewModel for a slot config.
    ///
    /// weaponMount  — parent Transform on the ship (weapon prefabs are placed here).
    /// playerTransform — the Player's own Transform (used by DroneModule for orbiting).
    /// </summary>
    public static class ActiveModuleFactory
    {
        public static WeaponModuleViewModel Create(
            WeaponSlotConfig slot,
            Transform weaponMount,
            Transform playerTransform)
        {
            var reloader = new SkillReloader(slot.Data.ShootDelay);

            IActiveSkill model = slot.ModuleType switch
            {
                WeaponModuleType.StandardGun  => BuildStandardGun(slot, weaponMount, reloader),
                WeaponModuleType.AutomaticGun => BuildAutomaticGun(slot, weaponMount, reloader),
                WeaponModuleType.MachineGun   => BuildMachineGun(slot, weaponMount, reloader),
                WeaponModuleType.RifleGun     => BuildRifleGun(slot, weaponMount, reloader),
                WeaponModuleType.AuraModule   => BuildAuraModule(slot, weaponMount, reloader),
                WeaponModuleType.DroneModule  => BuildDroneModule(slot, playerTransform, reloader),
                _ => null
            };

            if (model == null)
            {
                Debug.LogWarning($"[ActiveModuleFactory] Unknown or unsupported module type: {slot.ModuleType}");
                return null;
            }

            return new WeaponModuleViewModel(model, reloader);
        }

        // ── Guns ─────────────────────────────────────────────────────────────────

        private static IActiveSkill BuildStandardGun(
            WeaponSlotConfig slot, Transform parent, IReloadable reloader)
        {
            var view = SpawnGunView(slot, parent);
            if (view == null) return null;

            var model = new StandardGunModule();
            model.SetData(slot.Data);
            model.SetProjectileFactory(MakeFactory(slot, preloadCount: 10));
            model.SetReloader(reloader);
            model.SetEnemyDetector(new RaycastEnemyDetector(LayerMask.GetMask("Enemy")));
            model.SetPatterns(view.GetPatterns());
            model.RegisterDuplicator();
            return model;
        }

        private static IActiveSkill BuildAutomaticGun(
            WeaponSlotConfig slot, Transform parent, IReloadable reloader)
        {
            var view = SpawnGunView(slot, parent);
            if (view == null) return null;

            var patterns = view.GetPatterns();

            var model = new AutomaticGunModule();
            model.SetData(slot.Data);
            model.SetProjectileFactory(MakeFactory(slot, preloadCount: 10));
            model.SetReloader(reloader);
            model.SetEnemyDetector(new CircleFirstEnemyDetector(LayerMask.GetMask("Enemy")));
            if (patterns.Length > 0) model.SetPattern(patterns[0]);
            model.RegisterDuplicator();
            return model;
        }

        private static IActiveSkill BuildMachineGun(
            WeaponSlotConfig slot, Transform parent, IReloadable reloader)
        {
            var view = SpawnGunView(slot, parent);
            if (view == null) return null;

            var model = new MachineGunModule();
            model.SetData(slot.Data);
            model.SetProjectileFactory(MakeFactory(slot, preloadCount: 20));
            model.SetReloader(reloader);
            model.SetPatterns(view.GetPatterns());
            model.SetStartShotsPerCycle(slot.ShotsPerCycle);
            return model;
        }

        private static IActiveSkill BuildRifleGun(
            WeaponSlotConfig slot, Transform parent, IReloadable reloader)
        {
            var view = SpawnGunView(slot, parent);
            if (view == null) return null;

            var patterns = view.GetPatterns();

            var model = new RifleGunModule();
            model.SetData(slot.Data);
            model.SetProjectileFactory(MakeFactory(slot, preloadCount: 20));
            model.SetReloader(reloader);
            if (patterns.Length > 0) model.SetPattern(patterns[0]);
            model.SetStartShotsPerCycle(slot.ShotsPerCycle);
            return model;
        }

        // ── Special ──────────────────────────────────────────────────────────────

        private static IActiveSkill BuildAuraModule(
            WeaponSlotConfig slot, Transform parent, IReloadable reloader)
        {
            if (slot.ViewPrefab == null)
            {
                Debug.LogError("[ActiveModuleFactory] AuraModule slot requires a ViewPrefab " +
                               "with an AuraModuleView component.");
                return null;
            }

            var go = Object.Instantiate(slot.ViewPrefab, parent);
            var auraView = go.GetComponentInChildren<AuraModuleView>();

            if (auraView == null)
            {
                Debug.LogError("[ActiveModuleFactory] AuraModule ViewPrefab has no AuraModuleView " +
                               "component on it or its children.");
                return null;
            }

            auraView.Init(slot.Data.BulletData, slot.Damage, slot.CritChance, slot.CritMultiplier);

            var model = new AuraGunModule();
            model.SetData(slot.Data);
            model.SetReloader(reloader);
            model.SetView(go, auraView);
            return model;
        }

        private static IActiveSkill BuildDroneModule(
            WeaponSlotConfig slot, Transform playerTransform, IReloadable reloader)
        {
            // DroneModuleView is added at runtime parented to the player so drones orbit it.
            var viewGO = new GameObject("[DroneModuleView]");
            viewGO.transform.SetParent(playerTransform);
            viewGO.transform.localPosition = Vector3.zero;

            var droneView = viewGO.AddComponent<DroneModuleView>();

            var model = new DroneGunModule();
            model.SetData(slot.Data);
            model.SetReloader(reloader);
            model.SetView(
                droneView,
                slot.Data.BulletData,
                slot.Damage,
                slot.CritChance,
                slot.CritMultiplier,
                spawnPoint: viewGO.transform);

            if (slot.StartDroneCount > 0)
                model.SetStartSpawnCount(slot.StartDroneCount);

            return model;
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private static GunModuleView SpawnGunView(WeaponSlotConfig slot, Transform parent)
        {
            if (slot.ViewPrefab == null)
            {
                Debug.LogError($"[ActiveModuleFactory] Slot {slot.ModuleType} has no ViewPrefab assigned.");
                return null;
            }

            var go = Object.Instantiate(slot.ViewPrefab, parent);
            var view = go.GetComponent<GunModuleView>();

            if (view == null)
            {
                Debug.LogError($"[ActiveModuleFactory] ViewPrefab for {slot.ModuleType} " +
                               "has no GunModuleView component on its root.");
            }

            return view;
        }

        private static ProjectileFactory MakeFactory(WeaponSlotConfig slot, int preloadCount)
        {
            return new ProjectileFactory(
                slot.Data.BulletData,
                preloadCount,
                () => Object.Instantiate(slot.Data.BulletData.BulletObject).GetComponent<BaseBullet>());
        }
    }
}
