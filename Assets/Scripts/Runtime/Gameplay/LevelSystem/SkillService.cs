using System;
using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class SkillService : MonoBehaviour
    {
        private const int MAX_SKILLS_COUNT = 6;

        private const int MIN_SKILLS_GENERATE_COUNT = 3;
        private const int MAX_SKILLS_GENERATE_COUNT = 5;

        private const int CHEST_MIN_SKILLS = 1;
        private const int CHEST_MAX_SKILLS = 5;
        [SerializeField]
        private SkillConfig _skillConfig;
        private List<SkillType> _startAvailableSkills;
        private List<PassiveSkillData> _infinitySkills;

        private List<SkillType> _availableSkills;
        private List<ActiveSkill> _availableActiveSkills;
        private List<PassiveSkill> _availablePassiveSkills;

        private int _additionSkillGenerate = 0;//TODO Change take from config Skill Count Modificator

        private List<ActiveSkill> _activeSkills = new();
        private List<PassiveSkill> _passiveSkills = new();

        public void Construct(GameConfig gameConfig)
        {
            _skillConfig = gameConfig.SkillConfig;

        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _startAvailableSkills = _skillConfig.GetStartAvailableSkills();
            _infinitySkills = _skillConfig.GetInfinitySkills();
            _activeSkills = new List<ActiveSkill>();
            _passiveSkills = new List<PassiveSkill>();
        }

        private int GetChestSkillCount()
        {
            return UnityEngine.Random.Range(CHEST_MIN_SKILLS, CHEST_MAX_SKILLS + 1);
        }

        private int GetLevelUpSkillCount()
        {
            return MIN_SKILLS_GENERATE_COUNT + _additionSkillGenerate;
        }

        public void InitializeSkillPools()
        {
            _availableSkills = new List<SkillType>(_startAvailableSkills);
            _availableActiveSkills = new List<ActiveSkill>(_activeSkills);
            _availablePassiveSkills = new List<PassiveSkill>(_passiveSkills);
        }

        public List<PreparationSkillData> GetUpgradeOptions(bool isChest) 
        {
            List<PreparationSkillData> skillPreparationData = new List<PreparationSkillData>();
            int skillGenerationCount = isChest ? GetChestSkillCount() : GetLevelUpSkillCount();

            bool canCreateNewSkill = true;
            bool hasUpgrade = true;

            if (TryGetEvolution(out var evolutionSkill))
            {
                skillPreparationData.Add(evolutionSkill);
            }

            hasUpgrade = TryGetUpgrade(out var upgradeSkill);
            if (hasUpgrade)
            { 
                skillPreparationData.Add(upgradeSkill);
            }

            while (skillPreparationData.Count < skillGenerationCount)
            {
                if (!hasUpgrade && !canCreateNewSkill)
                {
                    break;
                } 

                if (hasUpgrade && canCreateNewSkill)
                {
                    if (UnityEngine.Random.value < 0.5f)
                    {
                        hasUpgrade = TryGetUpgrade(out var additionalUpgrade);
                        if (additionalUpgrade != null)
                        {
                            skillPreparationData.Add(additionalUpgrade);
                            continue;
                        }
                    }
                    else
                    {
                        canCreateNewSkill = TryGetNewSkill(out var newSkill);
                        if (newSkill != null)
                        {
                            skillPreparationData.Add(newSkill);
                            continue;
                        }
                    }
                }
                if(hasUpgrade && !canCreateNewSkill) 
                {
                    hasUpgrade = TryGetUpgrade(out var additionalUpgrade);
                    if (additionalUpgrade != null)
                    {
                        skillPreparationData.Add(additionalUpgrade);
                        continue;
                    }
                }
                if (!hasUpgrade && canCreateNewSkill)
                {
                    canCreateNewSkill = TryGetNewSkill(out var newSkill);
                    if (newSkill != null)
                    {
                        skillPreparationData.Add(newSkill);
                        continue;
                    }
                }
            }
            if(skillPreparationData.Count < skillGenerationCount) 
            {
                if (isChest)
                {
                    TryGetNewSkill(out var infinitAdditionSkill);
                    skillPreparationData.Add(infinitAdditionSkill);
                }
                else
                {
                    List<PreparationSkillData> additionalInfinitSKills = GetInfititySkill(skillGenerationCount - skillPreparationData.Count);
                    if (additionalInfinitSKills != null)
                        skillPreparationData.AddRange(additionalInfinitSKills);
                }
            }

            return skillPreparationData;
        }

        private PreparationSkillData AddOnlyGoldInfinitSkill(out PreparationSkillData infinitSkill) 
        {
            SkillData skillDataskillData = _infinitySkills[0]; //Todo нормальное взятие только для зотола 
            infinitSkill = new PreparationSkillData()
            {
                SkillType = skillDataskillData.Type,
                SkillSprite = skillDataskillData.SkillIcon,
                SkillUpgradeInfo = skillDataskillData.UpgradesInfo[0],
                ActivateSkillAction = ActiveteSkill
            };
            return infinitSkill;
        }

        private List<PreparationSkillData> GetInfititySkill(int requiredCount) 
        {
            List<PreparationSkillData> resultList = new List<PreparationSkillData>();

            int takeCount = Math.Min(requiredCount, _infinitySkills.Count);

            for (int i = 0; i < takeCount; i++)
            {
                SkillData skillDataskillData = _infinitySkills[i];
                var infinitSkill = new PreparationSkillData()
                {
                    SkillType = skillDataskillData.Type,
                    SkillSprite = skillDataskillData.SkillIcon,
                    SkillUpgradeInfo = skillDataskillData.UpgradesInfo[0],
                    ActivateSkillAction = ActiveteSkill
                };
                resultList.Add(infinitSkill);
            }

            return resultList;
        }

        private bool TryGetUpgrade(out PreparationSkillData upgradedSkill)
        {
            bool checkActiveFirst = UnityEngine.Random.value < 0.5f;

            if (checkActiveFirst)
            {
                if (TryGetActiveUpgrade(out upgradedSkill))
                    return true;

                return TryGetPassiveUpgrade(out upgradedSkill);
            }
            else
            {
                if (TryGetPassiveUpgrade(out upgradedSkill))
                    return true;

                return TryGetActiveUpgrade(out upgradedSkill);
            }
        }

        private bool TryGetActiveUpgrade(out PreparationSkillData activeUpgradedSkill)
        {
            activeUpgradedSkill = null;
            var upgradableSkills = _availableActiveSkills.Where(s => !s.IsMaxLevel()).ToList();

            if (upgradableSkills.Count > 0)
            {
                var selectedSkill = upgradableSkills[UnityEngine.Random.Range(0, upgradableSkills.Count)];
                activeUpgradedSkill = new PreparationSkillData()
                {
                    SkillType = selectedSkill.SkillData.Type,
                    SkillSprite = selectedSkill.SkillData.SkillIcon,
                    SkillUpgradeInfo = selectedSkill.SkillData.UpgradesInfo[selectedSkill.SkillLevel],
                    ActivateSkillAction = ActiveteSkill
                };
                _availableActiveSkills.Remove(selectedSkill);
                return true;
            }
            return false;
        }

        private bool TryGetPassiveUpgrade(out PreparationSkillData upgradedSkill)
        {
            upgradedSkill = null;
            var upgradableSkills = _availablePassiveSkills.Where(s => !s.IsMaxLevel()).ToList();

            if (upgradableSkills.Count > 0)
            {
                var selectedSkill = upgradableSkills[UnityEngine.Random.Range(0, upgradableSkills.Count)];
                upgradedSkill = new PreparationSkillData()
                {
                    SkillType = selectedSkill.SkillData.Type,
                    SkillSprite = selectedSkill.SkillData.SkillIcon,
                    SkillUpgradeInfo = selectedSkill.SkillData.UpgradesInfo[selectedSkill.SkillLevel],
                    ActivateSkillAction = ActiveteSkill
                };
                _availablePassiveSkills.Remove(selectedSkill);
                return true;
            }
            return false;
        }

        bool IsSkillActive(SkillType type) => _skillConfig.GetSkillByType(type).UseType == SkillUseType.Active;

        private bool TryGetNewSkill(out PreparationSkillData newSkill)
        {
            newSkill = null;

            bool canTakeActive = _activeSkills.Count < 5;
            bool canTakePassive = _passiveSkills.Count < 5;


            List<SkillType> availableSkills = _availableSkills
                .Where(skill =>
                    !_activeSkills.Any(a => a.GetSkillType() == skill)    // Не в активных
                    && !_passiveSkills.Any(p => p.GetSkillType() == skill) // Не в пассивных
                    && (IsSkillActive(skill) ? canTakeActive : canTakePassive) // Проверка лимита
                )
                .ToList();

            if (availableSkills.Count > 0)
            {
                var selectedType = availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];

                SkillData newSkillData = _skillConfig.GetSkillByType(selectedType);
                newSkill = new PreparationSkillData();
                newSkill.SkillType = newSkillData.Type;
                newSkill.SkillSprite = newSkillData.SkillIcon;
                newSkill.SkillUpgradeInfo = newSkillData.UpgradesInfo[0];
                newSkill.ActivateSkillAction = ActiveteSkill;
                newSkill.SkillActivationType = SkillActivationType.NewSkill;

                _availableSkills.Remove(selectedType);
                return true;
            }
            return false;
        }

        private bool TryGetEvolution(out PreparationSkillData evolutionSkill)
        {
            evolutionSkill = null;
            var availableEvolutions = _activeSkills
                .Where(activeSkill => activeSkill.CanEvolve() && _passiveSkills.Any(passiveSkill => passiveSkill.SkillData.Type == activeSkill.SkillData.Evolution.EvolutionSkillType && passiveSkill.IsMaxLevel()))
                .ToList();

            if (availableEvolutions.Count > 0)
            {
                ActiveSkillData data = availableEvolutions[UnityEngine.Random.Range(0, availableEvolutions.Count)].SkillData;
                evolutionSkill = new PreparationSkillData();
                evolutionSkill.SkillType = data.Type;
                evolutionSkill.SkillSprite = data.Evolution.EvolutionIcon;
                evolutionSkill.SkillUpgradeInfo = data.Evolution.EvolutionInfo;
                evolutionSkill.ActivateSkillAction = ActiveteSkill;
                evolutionSkill.SkillActivationType = SkillActivationType.Evolution;
                return true;
            }
            return false;
        }

        private void ActiveteSkill(SkillType type, SkillActivationType activationType) 
        {
            switch (activationType) 
            {
                case SkillActivationType.NewSkill:
                    HandleNewSkill(type);
                    break;

                case SkillActivationType.UpgradePassive:
                    HandleUpgradePassive(type);
                    break;

                case SkillActivationType.UpgradeActive:
                    HandleUpgradeActive(type);
                    break;

                case SkillActivationType.Evolution:
                    HandleEvolution(type);
                    break;
            }
        }

        private void HandleNewSkill(SkillType skillType)
        {
            var skillData = _skillConfig.GetSkillByType(skillType);

            if (skillData.UseType == SkillUseType.Active)
            {
                if (skillData is ActiveSkillData activeSkillData)
                {
                    var newActiveSkill = new ActiveSkill(activeSkillData);
                    _activeSkills.Add(newActiveSkill);
                }
            }
            else if (skillData.UseType == SkillUseType.Passive)
            {
                if (skillData is PassiveSkillData passiveSkillData)
                {
                    var newPassiveSkill = new PassiveSkill(passiveSkillData);
                    _passiveSkills.Add(newPassiveSkill);
                }
            }
        }

        private void HandleUpgradePassive(SkillType skillType)
        {
            var passiveSkillToUpgrade = _passiveSkills.FirstOrDefault(skill => skill.GetSkillType() == skillType);
            if (passiveSkillToUpgrade != null)
            {
                passiveSkillToUpgrade.UpgradeLevel();
            }
        }

        private void HandleUpgradeActive(SkillType skillType)
        {
            var activeSkillToUpgrade = _activeSkills.FirstOrDefault(skill => skill.GetSkillType() == skillType);
            if (activeSkillToUpgrade != null)
            {
                activeSkillToUpgrade.UpgradeLevel();
            }
        }

        private void HandleEvolution(SkillType skillType)
        {
            var skillToEvolve = _activeSkills.FirstOrDefault(skill => skill.GetSkillType() == skillType);

            if (skillToEvolve != null)
            {
                skillToEvolve.Evolution();
            }
        }

        public class PreparationSkillData
        {
            public Action<SkillType, SkillActivationType> ActivateSkillAction;
            public SkillActivationType SkillActivationType;
            public Sprite SkillSprite;
            public SkillType SkillType;
            public SkillUpgradeInfo SkillUpgradeInfo;
        }
    }
}


//public List<PreparationSkillData> GetUpgradeOptions(int additionalSkills)
//{
//    var upgradeOptions = new List<PreparationSkillData>();
//    int totalSlots = 3 + additionalSkills;
//    bool evolutionAdded = TryGetEvolution(out var evolutionSkill);
//    if (evolutionAdded) upgradeOptions.Add(evolutionSkill);

//    if (TryGetUpgrade(out var upgradeSkill)) upgradeOptions.Add(upgradeSkill);

//    while (upgradeOptions.Count < totalSlots)
//    {
//        if (_activeSkills.Count < 5 && _passiveSkills.Count < 5)
//        {
//            if (UnityEngine.Random.value > 0.5f && TryGetNewSkill(out var newSkill))
//            {
//                upgradeOptions.Add(newSkill);
//            }
//            else if (TryGetUpgrade(out var additionalUpgrade))
//            {
//                upgradeOptions.Add(additionalUpgrade);
//            }
//        }
//        else if (TryGetUpgrade(out var additionalUpgrade))
//        {
//            upgradeOptions.Add(additionalUpgrade);
//        }
//        else if (TryGetNewSkill(out var newSkill))
//        {
//            upgradeOptions.Add(newSkill);
//        }
//        else
//        {
//            upgradeOptions.Add(GetRandomInfinitySkill());
//        }
//    }
//    return upgradeOptions;
//}

//private bool TryGetEvolution(out PreparationSkillData evolutionSkill)
//{
//    evolutionSkill = null;
//    var availableEvolutions = _activeSkills.Where(s => (ActiveSkillData)s.SkillData.Evolution != null).ToList();
//    if (availableEvolutions.Count > 0)
//    {
//        evolutionSkill = availableEvolutions[UnityEngine.Random.Range(0, availableEvolutions.Count)];
//        return true;
//    }
//    return false;
//}

//private bool TryGetUpgrade(out PreparationSkillData upgradedSkill)
//{
//    upgradedSkill = null;
//    var availableUpgrades = _activeSkills.Cast<Skill>().Concat(_passiveSkills).Where(s => s.SkillData.Upgrades != null && s.SkillData.Upgrades.Count > 0).ToList();
//    if (availableUpgrades.Count > 0)
//    {
//        upgradedSkill = availableUpgrades[UnityEngine.Random.Range(0, availableUpgrades.Count)];
//        return true;
//    }
//    return false;
//}

//private bool TryGetNewSkill(out PreparationSkillData newSkill)
//{
//    newSkill = null;
//    var availableSkills = _startAvailableSkills.Except(_activeSkills.Select(s => s.GetSkillType())).Except(_passiveSkills.Select(s => s.GetSkillType()).ToList());
//    if (availableSkills.Count > 0)
//    {
//        var selectedType = availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];
//        newSkill = _skillConfig.GetSkillByType(selectedType);
//        if (newSkill is ActiveSkill active) _activeSkills.Add(active);
//        else if (newSkill is PassiveSkill passive) _passiveSkills.Add(passive);
//        return true;
//    }
//    return false;
//}