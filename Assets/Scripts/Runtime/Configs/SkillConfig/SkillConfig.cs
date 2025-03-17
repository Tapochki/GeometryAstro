using System;
using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.ConfigUtilities;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{

    [CreateAssetMenu(fileName = "SkillConfig", menuName = "TandC/Game/SkillConfig", order = 1)]
    public class SkillConfig : ScriptableObject, IJsonSerializable
    {
        [SerializeField] private bool _useTestSkills;
        [SerializeField] private List<ActiveSkill> _activeSkills;
        [SerializeField] private List<PassiveSkill> _passiveSkills;
        [SerializeField] private List<PassiveSkill> _additionSkills;

        [SerializeField] private List<SkillType> _startAvailableSkills;
        [SerializeField] private List<SkillType> _startAvailableTestSkills;

        public SkillData GetSkillByType(SkillType type)
        {
            return _activeSkills.FirstOrDefault(skill => skill.Type == type) as SkillData ??
                   _passiveSkills.FirstOrDefault(skill => skill.Type == type);
        }

        public List<SkillType> GetStartAvailableSkills()
        {
            return _useTestSkills ? _startAvailableSkills : _startAvailableTestSkills;
        }
    }

    [Serializable]
    public abstract class SkillData
    {
        public string SkillName;
        public Sprite SkillIcon;
        public SkillType Type;
        public SkillUseType UseType;
        public int MaxLevel => Upgrades?.Count ?? 0;
        public List<SkillUpgrade> Upgrades;
    }

    [Serializable]
    public class ActiveSkill : SkillData
    {
        public SkillEvolution Evolution;
        public SkillType ExclusionType;
    }

    [Serializable]
    public class PassiveSkill : SkillData
    {
        
    }

    [Serializable]
    public class SkillEvolution
    {
        public Sprite EvolutionIcon;
        public SkillType EvolutionSkillType;
        public string EvolutionName;
        public string EvolutionDescription;
    }

    [Serializable]
    public class SkillUpgrade
    {
        public string Name;
        public int Level;
        public string Description;
        public float Value;
    }
}