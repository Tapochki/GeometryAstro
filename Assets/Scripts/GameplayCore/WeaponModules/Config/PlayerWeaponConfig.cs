using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// ScriptableObject that defines which weapon modules the player starts with.
    ///
    /// Create via: Assets > Create > GameplayCore > Player Weapon Config
    ///
    /// Usage:
    ///   1. Create a PlayerWeaponConfig asset.
    ///   2. Fill Slots (up to 5). Set ModuleType, assign ViewPrefab and Data per slot.
    ///   3. Add ActiveModuleController to the Player GameObject and assign this config.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerWeaponConfig", menuName = "GameplayCore/Player Weapon Config")]
    public class PlayerWeaponConfig : ScriptableObject
    {
        [Tooltip("Up to 5 weapon slots. Slots with ModuleType.None are skipped.")]
        public WeaponSlotConfig[] Slots = new WeaponSlotConfig[5];
    }
}
