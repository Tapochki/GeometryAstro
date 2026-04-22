using UnityEngine;

namespace GameplayCore
{
    [System.Serializable]
    public class WeaponSlotConfig
    {
        [Tooltip("Set to None to leave this slot empty.")]
        public WeaponModuleType ModuleType;

        [Tooltip("Prefab with GunModuleView + WeaponShootingPattern children (gun types).\n" +
                 "Prefab with AuraModuleView + Trigger Collider2D child (AuraModule).\n" +
                 "Leave null for DroneModule — drone view is created at runtime.")]
        public GameObject ViewPrefab;

        public ActiveSkillData Data;

        [Header("Burst Options (MachineGun / RifleGun)")]
        [Tooltip("Number of shots fired per burst cycle.")]
        public int ShotsPerCycle = 3;

        [Header("Area / Orbit Options (AuraModule / DroneModule)")]
        [Tooltip("Base damage per hit or per aura tick.")]
        public float Damage = 10f;
        public float CritChance = 0f;
        public float CritMultiplier = 1.5f;

        [Header("Drone Options")]
        [Tooltip("Drones spawned per wave. 0 = use internal default (2).")]
        public int StartDroneCount = 0;
    }
}
