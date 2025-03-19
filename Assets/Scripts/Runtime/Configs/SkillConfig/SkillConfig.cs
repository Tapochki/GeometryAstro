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
        [Tooltip("If we use the test start date skills")]
        [SerializeField] private bool _useTestSkills;
        [Tooltip("List of Active Skills (weapons, drones, abilities)")]
        [SerializeField] private List<ActiveSkillData> _activeSkills;
        [Tooltip("List of Passive Skills/Modifiers")]
        [SerializeField] private List<PassiveSkillData> _passiveSkills;
        [Tooltip("Infinite Skill List. These are skils that we will use when all others are improved and there is no place to take new skils.")]
        [SerializeField] private List<PassiveSkillData> _additionSkills;

        [Tooltip("Here are all the skills that are available to us from the beginning of the game.")]
        [SerializeField] private List<SkillType> _startAvailableSkills;
        [Tooltip("Listed here are all the test skills that are available to us from the start of the game that we will be testing.")]
        [SerializeField] private List<SkillType> _startAvailableTestSkills;

        private void OnEnable()
        {
            _startAvailableSkills = GetSkillTypesFromSkills(_activeSkills.Cast<SkillData>().ToList())
                .Union(GetSkillTypesFromSkills(_passiveSkills.Cast<SkillData>().ToList()))
                .ToList();
        }

        private List<SkillType> GetSkillTypesFromSkills(List<SkillData> skills)
        {
            return skills.Select(skill => skill.Type).ToList();
        }

        public SkillData GetSkillByType(SkillType type)
        {
            return _activeSkills.FirstOrDefault(skill => skill.Type == type) as SkillData ??
                   _passiveSkills.FirstOrDefault(skill => skill.Type == type);
        }

        public List<SkillType> GetStartAvailableSkills()
        {
            return _useTestSkills ? _startAvailableTestSkills : _startAvailableSkills;
        }

        public List<PassiveSkillData> GetInfinitySkills()
        {
            return _additionSkills;
        }
    }

    [Serializable]
    public abstract class SkillData
    {
        [Tooltip("Skill Name. Will probably be used for the codex")]
        public string SkillName;
        [Tooltip("Skill Icon")]
        public Sprite SkillIcon;
        [Tooltip("Skill Type")]
        public SkillType Type;
        [Tooltip("Active or Passive")]
        public SkillUseType UseType;
        public int MaxLevel => UpgradesInfo?.Count ?? 0;
        [Tooltip("Skill Upgrades Information. 1 upgrade as well as information about the first registration of the skill")]
        public List<SkillUpgradeInfo> UpgradesInfo;
    }

    [Serializable]
    public class ActiveSkillData : SkillData
    {
        [Tooltip("Type of Active for improvement")]
        public ActiveSkillType ActiveSkillUpgradeType;
        [Tooltip("The type of the other skill that we will exclude from the available ones when we activate it")]
        public SkillType ExclusionSkillType;
        [Tooltip("Description of evolution")]
        public SkillEvolution EvolutionData;
    }

    [Serializable]
    public class PassiveSkillData : SkillData
    {
        [Tooltip("Type of Passive Skill/Modifier for improvement")]
        public ModificatorType ModificatorUpgradeType;
    }

    [Serializable]
    public class SkillEvolution
    {
        [Tooltip("We use a different picture for Evolution")]
        public Sprite EvolutionIcon;
        [Tooltip("Skill to interact with for evolution.")]
        public SkillType TypeForEvolution;
        [Tooltip("Description of evolution")]
        public SkillUpgradeInfo EvolutionInfo;
    }

    [Serializable]
    public class SkillUpgradeInfo
    {
        [Tooltip("Upgrade Key Title Name, Example KEY_[SkillName]_UPGRADE")]
        public string Name;
        [Tooltip("Skill Level for UI View"), Min(0)]
        public int Level;
        [Tooltip("Skill Key Upgrade Description. Example KEY_[SkillName]_UPGRADE_DESCRIPTION_LEVEL_[levelValue]")]
        public string Description;
        [Tooltip("Skill value is used for passive and also replaced in the text if there is %Value% in the text."), Min(0)]
        public float Value;
        [Tooltip("If the value is counted as a percentage.")]
        public bool IsPercentageValue;

        public string GetFormattedDescription(string description)
        {
            return description.Replace("%Value%", $"<b>{Value}</b>");
        }
    }
}