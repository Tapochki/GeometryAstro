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
        [SerializeField] private List<ActiveSkillData> _activeSkills;
        [SerializeField] private List<PassiveSkillData> _passiveSkills;
        [SerializeField] private List<PassiveSkillData> _additionSkills;

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

        public List<PassiveSkillData> GetInfinitySkills()
        {
            return _additionSkills;
        }
    }

    [Serializable]
    public abstract class SkillData
    {
        public string SkillName;
        public Sprite SkillIcon;
        public SkillType Type;
        public SkillUseType UseType;
        public int MaxLevel => UpgradesInfo?.Count ?? 0;
        public List<SkillUpgradeInfo> UpgradesInfo;
    }

    [Serializable]
    public class ActiveSkillData : SkillData
    {
        public SkillEvolution Evolution;
        public SkillType ExclusionType;
    }

    [Serializable]
    public class PassiveSkillData : SkillData
    {
        
    }

    [Serializable]
    public class SkillEvolution
    {
        public Sprite EvolutionIcon;
        public SkillType EvolutionSkillType;
        public SkillUpgradeInfo EvolutionInfo;
    }

    [Serializable]
    public class SkillUpgradeInfo
    {
        public string Name;
        public int Level;
        public string Description;
        public float Value;

        public string GetFormattedDescription()
        {
            return Description.Replace("%Value%", $"<b>{Value}</b>");
        }
    }
}