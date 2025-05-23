using System;
using System.Collections.Generic;
using TandC.GeometryAstro.ConfigUtilities;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "ActiveSkillConfig", menuName = "TandC/Game/ActiveSkillConfig", order = 1)]
    public class ActiveSkillConfig : ScriptableObject, IJsonSerializable
    {
        [SerializeField] private List<ActiveSkillData> _activeSkillsData;

        [SerializeField] private AdditionalSkillConfig _additionalSkillConfig;

        public AdditionalSkillConfig AdditionalSkillConfig => _additionalSkillConfig;

        public ActiveSkillData GetActiveSkillByType(ActiveSkillType activeSkillType)
        {
            foreach (var activeSkill in _activeSkillsData)
            {
                if (activeSkill.type == activeSkillType)
                {
                    return activeSkill;
                }
            }

            return null;
        }

    }

    [Serializable]
    public class ActiveSkillData
    {
        public string SkillName;
        public ActiveSkillType type;
        public float shootDeley;
        public float detectorRadius;
        public BulletData bulletData;
        public BulletData EvolvedBulletData;
        public GameObject _skillPrefab;
    }

    [Serializable]
    public class BulletData
    {
        public int BulletSpeed;
        public float bulletLifeTime;
        public int bulletLife;
        public float baseDamage;
        public float BasicCriticalChance;
        public float BasicCriticalMultiplier;
        public float BasicBulletSize;
        public GameObject BulletObject;
        public ActiveSkillType type;
    }
}