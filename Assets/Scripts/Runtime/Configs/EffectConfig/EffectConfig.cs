using TandC.GeometryAstro.ConfigUtilities;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "EffectsConfig", menuName = "TandC/Game/EffectsConfig", order = 10)]
    public class EffectsConfig : ScriptableObject, IJsonSerializable
    {
        public DamageEffectConfig DamageEffectConfig => _damageEffectConfig;
        public ExplosionEffectConfig ExplosionEffectConfig => _explosionEffectConfig;
        public EnemyDeathEffectConfig EnemyDeathEffectConfig => _enemyDeathEffectConfig;
        public FreezeEffectConfig FreezeEffectConfig => _freezeEffectConfig;
        public DamageAreaEffectConfig DamageAreaEffectConfig => _damageAreaEffectConfig;

        [SerializeField] DamageEffectConfig _damageEffectConfig;
        [SerializeField] ExplosionEffectConfig _explosionEffectConfig;
        [SerializeField] EnemyDeathEffectConfig _enemyDeathEffectConfig;
        [SerializeField] FreezeEffectConfig _freezeEffectConfig;
        [SerializeField] DamageAreaEffectConfig _damageAreaEffectConfig;
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

    [System.Serializable]
    public class EnemyDeathEffectConfig : BaseEffectConfig
    {

    }

    [System.Serializable]
    public class FreezeEffectConfig : BaseEffectConfig
    {

    }

    [System.Serializable]
    public class DamageAreaEffectConfig : BaseEffectConfig
    {

    }
}
