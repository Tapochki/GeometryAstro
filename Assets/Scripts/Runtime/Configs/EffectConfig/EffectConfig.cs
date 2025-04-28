using TandC.GeometryAstro.ConfigUtilities;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "EffectsConfig", menuName = "TandC/Game/EffectsConfig", order = 10)]
    public class EffectsConfig : ScriptableObject, IJsonSerializable
    {
        public DamageEffectConfig DamageEffectConfig => _damageEffectConfig;
        public ExplosionEffectConfig ExplosionEffectConfig => _explosionEffectConfig;

        [SerializeField] DamageEffectConfig _damageEffectConfig;
        [SerializeField] ExplosionEffectConfig _explosionEffectConfig;
    }

    public class BaseEffectConfig 
    {
        public int startPreloadCount;
        public GameObject effectObject;
    }

    [System.Serializable]
    public class DamageEffectConfig : BaseEffectConfig
    {

        public Color StandartColor;
        public Color CritColor;
        public float LifeTime;
    }

    [System.Serializable]
    public class ExplosionEffectConfig : BaseEffectConfig
    {
       
    }
}
