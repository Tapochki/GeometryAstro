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
        public DashConfig DashConfig => _dashConfig;
        public LaserConfig LaserConfig => _laserConfig;
        public MiniGunConfig MinigunConfig => _miniGunConfig;
        public RifleGunConfig RifleConfig => _rifleGunConfig;

        [SerializeField] ShieldColorConfig _shieldColorConfig;
        [SerializeField] AreaEffectConfig _areaEffectConfig;
        [SerializeField] CloakConfig _cloakConfig;
        [SerializeField] RocketConfig _rocketConfig;
        [SerializeField] DashConfig _dashConfig;
        [SerializeField] LaserConfig _laserConfig;
        [SerializeField] MiniGunConfig _miniGunConfig;
        [SerializeField] RifleGunConfig _rifleGunConfig;
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

    [System.Serializable]
    public class DashConfig
    {
        public float StartDashMultiplier;
        public float DashTime;
        public float FireTraceSpawnTimer;
        public Color EvolvedColor;
    }

    [System.Serializable]
    public class LaserConfig
    {
        public float LaserActiveTime;
    }

    [System.Serializable]
    public class MiniGunConfig
    {
        public int StartShootPreCycleCount;
    }

    [System.Serializable]
    public class RifleGunConfig
    {
        public int StartShootCount;
    }
}
