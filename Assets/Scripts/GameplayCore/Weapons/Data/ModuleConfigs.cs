using UnityEngine;

namespace GameplayCore
{
    [CreateAssetMenu(fileName = "AutomaticGunConfig", menuName = "GameplayCore/Automatic Gun Config")]
    public class AutomaticGunConfig : ScriptableObject
    {
        public ActiveSkillData Data;
    }

    [CreateAssetMenu(fileName = "MachineGunConfig", menuName = "GameplayCore/Machine Gun Config")]
    public class MachineGunConfig : ScriptableObject
    {
        public ActiveSkillData Data;

        [Tooltip("How many shots fire per burst cycle.")]
        public int StartShotsPerCycle = 5;
    }

    [CreateAssetMenu(fileName = "RifleGunConfig", menuName = "GameplayCore/Rifle Gun Config")]
    public class RifleGunConfig : ScriptableObject
    {
        public ActiveSkillData Data;

        [Tooltip("How many shots fire per burst cycle.")]
        public int StartShotCount = 3;
    }

    [CreateAssetMenu(fileName = "DroneModuleConfig", menuName = "GameplayCore/Drone Module Config")]
    public class DroneModuleConfig : ScriptableObject
    {
        public ActiveSkillData Data;

        [Tooltip("Base damage per drone hit.")]
        public float Damage = 10f;
        public float CritChance = 0f;
        public float CritMultiplier = 1.5f;
    }

    [CreateAssetMenu(fileName = "AuraModuleConfig", menuName = "GameplayCore/Aura Module Config")]
    public class AuraModuleConfig : ScriptableObject
    {
        public ActiveSkillData Data;

        [Tooltip("Base damage applied to each enemy inside the aura per tick.")]
        public float Damage = 5f;
        public float CritChance = 0f;
        public float CritMultiplier = 1.5f;

        [Tooltip("How much each Upgrade() call increases the aura collider scale.")]
        public float UpgradeSizeStep = 0.5f;
    }
}
