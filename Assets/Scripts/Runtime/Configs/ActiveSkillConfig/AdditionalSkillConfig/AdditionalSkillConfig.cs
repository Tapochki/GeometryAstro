using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "AdditionalSkillConfig", menuName = "TandC/Game/AdditionalSkillConfig", order = 10)]
    public class AdditionalSkillConfig : ScriptableObject
    {
        public ShieldColorConfig ShieldColorConfig => _shieldColorConfig;
        public AreaEffectConfig AreaEffectConfig => _areaEffectConfig;
        public CloakConfig CloakConfig => _cloakConfig;

        [SerializeField] ShieldColorConfig _shieldColorConfig;
        [SerializeField] AreaEffectConfig _areaEffectConfig;
        [SerializeField] CloakConfig _cloakConfig;
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

}
