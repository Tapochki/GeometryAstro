using System;
using System.Collections.Generic;
using TandC.Settings;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TandC.Data
{
    [CreateAssetMenu(fileName = "SkillConfig", menuName = "TandC/Game/SkillConfig", order = 1)]
    public class SkillConfig : ScriptableObject
    {
        [SerializeField] private List<SkillData> _skillData;

        public List<SkillType> StartSkills;

        public List<SkillType> StartAvailableSkills;

        public List<SkillType> StartTestAvailableSkills;

        public SkillData GetSkillByType(SkillType skillType)
        {
            foreach (var item in _skillData)
            {
                if (item.SkillDescription.type == skillType)
                {
                    return item;
                }
            }

            return null;
        }

        public void SetUpgradeDescription() 
        {
            foreach(var item in _skillData) 
            {
                foreach(var skillUpgrade in item.UpgradeList) 
                {
                    skillUpgrade.SkillDescription = item.SkillDescription;
                }
            }
        }
    }

    [Serializable]
    public class SkillDescription 
    {
        public uint id;
        public string name;
        [TextArea(5, 10)]
        public string skillDescription;
        public Sprite skillIcon;
        public SkillType type;
    }

    [Serializable]
    public class SkillData
    {
        public SkillDescription SkillDescription;
        public int MaxLevel => UpgradeList.Count;       
        public SkillUseType useType;
        public string description;
        public List<SkillUpgradeData> UpgradeList;
    }

    [Serializable]
    public class SkillUpgradeData 
    {
        public SkillDescription SkillDescription;
        public float Value;
    }
}