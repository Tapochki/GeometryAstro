using TandC.GeometryAstro.ConfigUtilities;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "AdditionalSkillConfig", menuName = "TandC/Game/AdditionalSkillConfig", order = 10)]
    public class AdditionalSkillConfig : ScriptableObject, IJsonSerializable
    {
        public ShieldColorConfig ShieldColorConfig => _shieldColorConfig;
        public AreaEffectConfig AreaEffectConfig => _areaEffectConfig;
        public CloakConfig CloakConfig => _cloakConfig;
        public RocketConfig RocketConfig => _rocketConfig;

        [SerializeField] ShieldColorConfig _shieldColorConfig;
        [SerializeField] AreaEffectConfig _areaEffectConfig;
        [SerializeField] CloakConfig _cloakConfig;
        [SerializeField] RocketConfig _rocketConfig;
    }

    [System.Serializable]
    public class CloakConfig
    {
        public float StartCloakActiveTime;
    }

    [System.Serializable]
    public class ShieldColorConfig
    {
        [Header("Shield Colors")]
        public Color[] shieldColors = new Color[5];
    }

    [System.Serializable]
    public class AreaEffectConfig
    {
        [Header("Area damage interval")]
        public float damageInterval;
    }

    [System.Serializable]
    public class RocketConfig
    {
        public int StartRocketCount;
    }
}
