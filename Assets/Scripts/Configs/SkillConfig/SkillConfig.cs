using System;
using System.Collections.Generic;
using TandC.Settings;
using UnityEngine;

namespace TandC.Data
{
    [CreateAssetMenu(fileName = "SkillConfig", menuName = "TandC/Game/SkillConfig", order = 1)]
    public class SkillConfig : ScriptableObject
    {
        [SerializeField] private List<SkillsData> _skillData;

        public SkillsData GetSkillByType(SkillType skillType)
        {
            foreach (var item in _skillData)
            {
                if (item.type == skillType)
                {
                    return item;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class SkillsData
    {
        public uint id;
        public string name;
        public string nameForDev;
        public Sprite sprite;
        public int MaxLevel;
        public float Value = 0;
        public SkillType type;
        public SkillUseType useType;
        public string description;
        public bool isIncrease;
        public bool isProcent;
        public float procentIncrease;
    }
}