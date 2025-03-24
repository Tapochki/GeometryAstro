using System;
using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class SkillService
    {
        private const int MIN_SKILLS_GENERATE_COUNT = 3;
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

        private Dictionary<SkillActivationType, Action<SkillType>> _skillHandlers;

        private SkillsView _skillsView;

        [Inject]
        public void Construct(GameConfig gameConfig, SkillsView skillsView)
        {
            _skillConfig = gameConfig.SkillConfig;
            _skillsView = skillsView;
        }

        public void Initialize()
        {
            InitializeSkillActivationHandlers();
            _startAvailableSkills = _skillConfig.GetStartAvailableSkills();
            _infinitySkills = _skillConfig.GetInfinitySkills();
            _skillsView.Init();
            _activeSkills = new List<ActiveSkill>();
            _passiveSkills = new List<PassiveSkill>();
        }

        public void InitializeSkillActivationHandlers()
        {
            _skillHandlers = new Dictionary<SkillActivationType, Action<SkillType>>{
                { SkillActivationType.NewSkill, HandleNewSkill },
                { SkillActivationType.UpgradePassive, HandleUpgradePassive },
                { SkillActivationType.UpgradeActive, HandleUpgradeActive },
                { SkillActivationType.Evolution, HandleEvolution },
                { SkillActivationType.InfinitSkill, HandleInfinitSkill }
            };
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
            InitializeSkillPools();
            bool canCreateNewSkill = !isChest; // in chest we cannot add new skill
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
                if (hasUpgrade && !canCreateNewSkill)
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
            if (skillPreparationData.Count < skillGenerationCount)
            {
                if (isChest)
                {
                    AddOnlyGoldInfinitSkill(out var infinitAdditionSkill);
                    skillPreparationData.Add(infinitAdditionSkill);
                }
                else
                {
                    List<PreparationSkillData> additionalInfinitSKills = GetInfititySkill(skillGenerationCount - skillPreparationData.Count);
                    if (additionalInfinitSKills != null)
                    {
                        skillPreparationData.AddRange(additionalInfinitSKills);
                    }
                }
            }

            return skillPreparationData;
        }

        private PreparationSkillData AddOnlyGoldInfinitSkill(out PreparationSkillData goldInfinitSkill)
        {
            SkillData skillDataskillData = _infinitySkills[0]; //Todo нормальное взятие только для денег 
            goldInfinitSkill = CreatePreparationSkill(SkillActivationType.InfinitSkill, skillDataskillData.Type, skillDataskillData.SkillIcon, skillDataskillData.UpgradesInfo[0]);
            return goldInfinitSkill;
        }

        private List<PreparationSkillData> GetInfititySkill(int requiredCount)
        {
            List<PreparationSkillData> resultList = new List<PreparationSkillData>();

            int takeCount = Math.Min(requiredCount, _infinitySkills.Count);

            for (int i = 0; i < takeCount; i++)
            {
                SkillData skillDataskillData = _infinitySkills[i];
                PreparationSkillData infinitSkill = CreatePreparationSkill(SkillActivationType.InfinitSkill, skillDataskillData.Type, skillDataskillData.SkillIcon, skillDataskillData.UpgradesInfo[0]);
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
                {
                    return true;
                }

                return TryGetPassiveUpgrade(out upgradedSkill);
            }
            else
            {
                if (TryGetPassiveUpgrade(out upgradedSkill))
                {
                    return true;
                }

                return TryGetActiveUpgrade(out upgradedSkill);
            }
        }

        private bool TryGetActiveUpgrade(out PreparationSkillData activeUpgradedSkill)
        {
            activeUpgradedSkill = null;
            if (_availableActiveSkills == null)
            {
                return false;
            }
            var upgradableSkills = GetActiveSkillsNotReachedMaxLevel();

            if (upgradableSkills.Count > 0)
            {
                var selectedSkill = upgradableSkills[UnityEngine.Random.Range(0, upgradableSkills.Count)];

                activeUpgradedSkill = CreatePreparationSkill(SkillActivationType.UpgradeActive, selectedSkill.SkillData.Type, selectedSkill.SkillData.SkillIcon, selectedSkill.SkillData.UpgradesInfo[selectedSkill.SkillLevel]);
                _availableActiveSkills.Remove(selectedSkill);
                return true;
            }
            return false;
        }

        private List<ActiveSkill> GetActiveSkillsNotReachedMaxLevel() 
        {
            return _availableActiveSkills.Where(s => !s.IsMaxLevel()).ToList();
        }

        private bool TryGetPassiveUpgrade(out PreparationSkillData upgradedPassiveSkill)
        {
            upgradedPassiveSkill = null;
            if (_availablePassiveSkills == null)
            {
                return false;
            }
            var upgradableSkills = GetPassiveSkillsNotReachedMaxLevel();

            if (upgradableSkills.Count > 0)
            {
                var selectedSkill = upgradableSkills[UnityEngine.Random.Range(0, upgradableSkills.Count)];

                upgradedPassiveSkill = CreatePreparationSkill(SkillActivationType.UpgradePassive, selectedSkill.SkillData.Type, selectedSkill.SkillData.SkillIcon, selectedSkill.SkillData.UpgradesInfo[selectedSkill.SkillLevel]);
                _availablePassiveSkills.Remove(selectedSkill);
                return true;
            }
            return false;
        }

        private List<PassiveSkill> GetPassiveSkillsNotReachedMaxLevel()
        {
            return _availablePassiveSkills.Where(s => !s.IsMaxLevel()).ToList();
        }

        private bool IsSkillActive(SkillType type) => _skillConfig.GetSkillByType(type).UseType == SkillUseType.Active;

        private bool TryGetNewSkill(out PreparationSkillData newSkill)
        {
            newSkill = null;

            List<SkillType> availableSkills = GetAvailableSkills();

            if (availableSkills.Count > 0)
            {
                var selectedType = availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];

                SkillData newSkillData = _skillConfig.GetSkillByType(selectedType);
                newSkill = CreatePreparationSkill(SkillActivationType.NewSkill, newSkillData.Type, newSkillData.SkillIcon, newSkillData.UpgradesInfo[0]);

                _availableSkills.Remove(selectedType);
                return true;
            }
            return false;
        }

        private List<SkillType> GetAvailableSkills()
        {
            bool canTakeActive = IsMaximumActiveSkill();
            bool canTakePassive = IsMaximumPassiveSkill();

            return _availableSkills
                .Where(skill => IsSkillAvailable(skill, canTakeActive, canTakePassive))
                .ToList();
        }

        private bool IsMaximumActiveSkill() => _activeSkills.Count < 5;

        private bool IsMaximumPassiveSkill() => _passiveSkills.Count < 5;

        private bool IsSkillAvailable(SkillType skill, bool canTakeActive, bool canTakePassive) => !IsSkillAlreadyTaken(skill) && (IsSkillActive(skill) ? canTakeActive : canTakePassive);

        private bool IsSkillAlreadyTaken(SkillType skill) => _activeSkills.Any(a => a.GetSkillType() == skill) || _passiveSkills.Any(p => p.GetSkillType() == skill);

        private bool TryGetEvolution(out PreparationSkillData evolutionSkill)
        {
            evolutionSkill = null;
            List<ActiveSkill> availableEvolutions = SortSkillWhichCanEvolve();

            if (availableEvolutions.Count > 0)
            {
                ActiveSkillData data = availableEvolutions[UnityEngine.Random.Range(0, availableEvolutions.Count)].SkillData;
                evolutionSkill = CreatePreparationSkill(SkillActivationType.Evolution, data.Type, data.EvolutionData.EvolutionIcon, data.EvolutionData.EvolutionInfo);
                return true;
            }
            return false;
        }

        private List<ActiveSkill> SortSkillWhichCanEvolve()
        {
            return _activeSkills
                .Where(activeSkill => CanSkillEvolve(activeSkill))
                .ToList();
        }

        private bool CanSkillEvolve(ActiveSkill activeSkill)
        {
            return activeSkill.CanEvolve() && HasRequiredMaxLevelPassiveSkill(activeSkill);
        }

        private bool HasRequiredMaxLevelPassiveSkill(ActiveSkill activeSkill)
        {
            return _passiveSkills.Any(passiveSkill =>
                IsRequiredPassiveSkill(passiveSkill, activeSkill) && passiveSkill.IsMaxLevel());
        }

        private bool IsRequiredPassiveSkill(PassiveSkill passiveSkill, ActiveSkill activeSkill)
        {
            return passiveSkill.SkillData.Type == activeSkill.SkillData.EvolutionData.TypeForEvolution;
        }

        private PreparationSkillData CreatePreparationSkill(SkillActivationType activationType, SkillType type, Sprite skillIcon, SkillUpgradeInfo skillInfo)
        {
            PreparationSkillData preparationSkill = new PreparationSkillData();
            preparationSkill.SkillType = type;
            preparationSkill.SkillSprite = skillIcon;
            preparationSkill.SkillUpgradeInfo = skillInfo;
            preparationSkill.ActivateSkillAction = ActivateSkill;
            preparationSkill.SkillActivationType = activationType;

            return preparationSkill;
        }

        private void ActivateSkill(SkillType type, SkillActivationType activationType)
        {
            if (_skillHandlers.TryGetValue(activationType, out var handler))
            {
                handler.Invoke(type);
            }
            else
            {
                Debug.LogError($"Unknown type of skill activation: {activationType}");
            }
        }
        #region Test
        private void ShowCurrentActiveSkills()
        {
            string activeSkillsInfo = "Active Skills:\n";

            foreach (var skill in _activeSkills)
            {
                activeSkillsInfo += $"<color=red>{skill.SkillData.SkillName}</color> " +
                                    $"<color=blue>Level {skill.SkillLevel}</color>\n";
            }

            Debug.LogError(activeSkillsInfo);
        }

        private void ShowCurrentPassiveSkills()
        {
            string passiveSkillsInfo = "Passive Skills:\n";

            foreach (var skill in _passiveSkills)
            {
                passiveSkillsInfo += $"<color=red>{skill.SkillData.SkillName}</color> " +
                                     $"<color=blue>Level {skill.SkillLevel}</color>\n";
            }

            Debug.LogError(passiveSkillsInfo);
        }
        #endregion
        #region Apply Skills
        private void HandleNewSkill(SkillType skillType)
        {
            var skillData = _skillConfig.GetSkillByType(skillType);

            if (skillData.UseType == SkillUseType.Active)
            {
                if (skillData is ActiveSkillData activeSkillData)
                {
                    HandleNewActiveSkill(activeSkillData, skillType);
                }
            }
            else if (skillData.UseType == SkillUseType.Passive)
            {
                if (skillData is PassiveSkillData passiveSkillData)
                {
                    HandleNewPassiveSKill(passiveSkillData, skillType);
                }
            }
        }

        private void HandleNewActiveSkill(ActiveSkillData activeSkillData, SkillType skillType)
        {
            var newActiveSkill = new ActiveSkill(activeSkillData, activeSkillData.ActiveSkillUpgradeType);
            _activeSkills.Add(newActiveSkill);
            AddSkillItemToView(SkillUseType.Active, skillType, newActiveSkill.SkillData.SkillIcon);
        }

        private void HandleNewPassiveSKill(PassiveSkillData passiveSkillData, SkillType skillType)
        {
            var newPassiveSkill = new PassiveSkill(passiveSkillData, passiveSkillData.ModificatorUpgradeType);
            _passiveSkills.Add(newPassiveSkill);
            AddSkillItemToView(SkillUseType.Passive, skillType, newPassiveSkill.SkillData.SkillIcon);
        }

        private void AddSkillItemToView(SkillUseType skillUseType, SkillType skillType, Sprite sprite) 
        {
            _skillsView.AddSkillItem(skillUseType, skillType, sprite);
        }

        private void HandleUpgradePassive(SkillType skillType)
        {
            var passiveSkillToUpgrade = _passiveSkills.FirstOrDefault(skill => skill.GetSkillType() == skillType);
            if (passiveSkillToUpgrade != null)
            {
                passiveSkillToUpgrade.ApplySkillEffect();
            }
        }

        private void HandleUpgradeActive(SkillType skillType)
        {
            var activeSkillToUpgrade = _activeSkills.FirstOrDefault(skill => skill.GetSkillType() == skillType);
            if (activeSkillToUpgrade != null)
            {
                activeSkillToUpgrade.ApplySkillEffect();
            }
        }

        private void HandleInfinitSkill(SkillType skillType)
        {

        }

        private void HandleEvolution(SkillType skillType)
        {
            var skillToEvolve = _activeSkills.FirstOrDefault(skill => skill.GetSkillType() == skillType);

            if (skillToEvolve != null)
            {
                skillToEvolve.Evolve();
            }

            _skillsView.UpdateSkillSprite(skillToEvolve.SkillData.UseType, skillType, skillToEvolve.SkillData.EvolutionData.EvolutionIcon);
        }
        #endregion

    }
    public class PreparationSkillData
    {
        public Action<SkillType, SkillActivationType> ActivateSkillAction;

        public SkillActivationType SkillActivationType;
        public SkillType SkillType;

        public Sprite SkillSprite;
        public SkillUpgradeInfo SkillUpgradeInfo;

        public void ApplySkill()
        {
            ActivateSkillAction?.Invoke(SkillType, SkillActivationType);
        }
    }
}