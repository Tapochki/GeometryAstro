using System;
using System.Collections.Generic;
using System.Linq;
using TandC.Data;
using TandC.Settings;
using UnityEngine;
using Zenject;

namespace TandC.Gameplay 
{
    public class SkillService : MonoBehaviour, ISkillService
    {
        private const int MAX_SKILLS_COUNT = 6;

        private const int MIN_SKILLS_GENERATE_COUNT = 3;
        private const int MAX_SKILLS_GENERATE_COUNT = 4;

        private SkillConfig _skillConfig;

        private List<SkillType> _availableSkills;

        private List<ActiveSkill> _activeSkills;
        private List<PassiveSkill> _passiveSkills;

        private List<Skill> _skillForUpgrade;

        private SkillFactory _skillFactory;

        [Inject]
        private void Construct(SkillConfig skillConfig, SkillFactory skillFactory)
        {
            _skillConfig = skillConfig;
            _skillFactory = skillFactory;
        }

        private void Start()
        {
            InitializeStartSkills();
        }

        public void StartGenerateSkills()
        {
            int generateSkillCount = _availableSkills.Count + _skillForUpgrade.Count;
            if (generateSkillCount < MIN_SKILLS_GENERATE_COUNT) 
            {
                GenerateRandomSkills(generateSkillCount);
            }
            else if(generateSkillCount == 0) 
            {
                SendInfinitySkills();
            }
            else 
            {
                generateSkillCount = RandomPreparationSkillCount();
                GenerateRandomSkills(generateSkillCount);
            }                    
        }

        private void GenerateRandomSkills(int generateSkillCount) 
        {
            bool isMaxSkills = IsMaxActiveSkill() && IsMaxPassiveSkill();
            List<PreparationSkillData> preparationSkillDatas = new List<PreparationSkillData>();
            List<SkillType> tempAvailableSkillTypes = new List<SkillType>(_availableSkills);
            List<Skill> tempSkillForUpgrade = new List<Skill>(_skillForUpgrade);

            for (int i = 0; i < generateSkillCount; i++)
            {
                PreparationSkillData skillPreparation;
                int random = UnityEngine.Random.Range(0, 4);
                if (isMaxSkills && random < 2)
                {
                    skillPreparation = PrepareDataForUpgradeSkill(tempSkillForUpgrade);
                }
                else
                {
                    skillPreparation = PrepareDataForNewSkill(tempAvailableSkillTypes);
                }
                preparationSkillDatas.Add(skillPreparation);
            }
            SendPreparationSkillDataToView(preparationSkillDatas);
        }

        private void InitializeStartSkills()
        {
            foreach(SkillType skillType in _skillConfig.StartSkills) 
            {
                ApplyNewSkill(skillType);
            }
        }

        private void SendInfinitySkills() 
        {

        }

        private void SendPreparationSkillDataToView(List<PreparationSkillData> preparationSkillDatas) 
        {

        }

        private int RandomPreparationSkillCount()
        {
            int random = UnityEngine.Random.Range(0, 101);
            int weightOfLuck = 5; // Later change to LuckSkill
            return random <= weightOfLuck ? MAX_SKILLS_GENERATE_COUNT : MIN_SKILLS_GENERATE_COUNT;
        }

        private void ApplyUpgradeSkill(SkillType type)
        {
            Skill skillForUpgrade = _skillForUpgrade.FirstOrDefault(skill => skill.GetSkillType() == type);
            if (skillForUpgrade != null)
            {
                skillForUpgrade.ApplySkillEffect();
                UpdateUpgradesSkillList();
            }
        }

        private void ApplyNewSkill(SkillType type)
        {
            Skill skill = _skillFactory.CreateSkill(GetNewSkillData(type));
            _availableSkills.Remove(type);
            _skillForUpgrade.Add(skill);
        }

        private PreparationSkillData PrepareDataForUpgradeSkill(List<Skill> tempSkillForUpgrade)
        {
            PreparationSkillData skillPreparation = new PreparationSkillData();
            Skill skill = GetExistingSkill(tempSkillForUpgrade);
            skillPreparation.SkillDescription = skill.GetUpgradeData();
            skillPreparation.ActivateSkillAction = ApplyUpgradeSkill;
            return skillPreparation;
        }

        private PreparationSkillData PrepareDataForNewSkill(List<SkillType> tempAvailableSkillTypes)
        {
            PreparationSkillData skillPreparation = new PreparationSkillData();

            SkillType skillType = tempAvailableSkillTypes[UnityEngine.Random.Range(0, tempAvailableSkillTypes.Count)];
            tempAvailableSkillTypes.Remove(skillType);

            skillPreparation.SkillDescription = GetNewSkillData(skillType).SkillDescription;
            skillPreparation.ActivateSkillAction = ApplyNewSkill;
            return skillPreparation;
        }

        private SkillData GetNewSkill()
        {
            SkillType skillType = _availableSkills[UnityEngine.Random.Range(0, _availableSkills.Count)];
            return GetNewSkillData(skillType);
        }

        private SkillData GetNewSkillData(SkillType type)
        {
            return _skillConfig.GetSkillByType(type);
        }

        private bool IsMaxActiveSkill()
        {
            return _activeSkills.Count <= MAX_SKILLS_COUNT;
        }

        private bool IsMaxPassiveSkill()
        {
            return _passiveSkills.Count <= MAX_SKILLS_COUNT;
        }

        private void UpdateUpgradesSkillList()
        {
            _skillForUpgrade.RemoveAll(skill => skill.IsMaxLevel());
        }

        private Skill GetExistingSkill(List<Skill> tempSkillForUpgrade)
        {
            int random = UnityEngine.Random.Range(0, tempSkillForUpgrade.Count);
            return tempSkillForUpgrade[random];
        }

        public class PreparationSkillData
        {
            public Action<SkillType> ActivateSkillAction;
            public SkillDescription SkillDescription;
        }
    }
}

