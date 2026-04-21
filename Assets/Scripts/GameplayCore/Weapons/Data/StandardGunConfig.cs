using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// ScriptableObject config for the StandardGun.
    /// Create via: Assets > Create > GameplayCore > Standard Gun Config
    /// </summary>
    [CreateAssetMenu(fileName = "StandardGunConfig", menuName = "GameplayCore/Standard Gun Config")]
    public class StandardGunConfig : ScriptableObject
    {
        public ActiveSkillData Data;
    }
}
