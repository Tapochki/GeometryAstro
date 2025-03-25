using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEditor;
using UnityEngine;

public class SkillConfigWindow : EditorWindow
{
    private SkillConfig _config;
    private SkillType _selectedType;
    private SkillUpgradeData _currentSkill;
    private bool _isNewSkill;

    private int _copyCount = 1;
    private SkillUpgradeInfo _templateUpgrade;

    private Vector2 _scrollPosition;
    private bool _showUpgrades = true;
    private List<bool> _upgradeFoldouts = new List<bool>();

    [MenuItem("Tools/Skill Config Editor")]
    public static void ShowWindow()
    {
        GetWindow<SkillConfigWindow>("Skill Config Editor");
    }

    private void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        if (_config == null)
        {
            _config = (SkillConfig)EditorGUILayout.ObjectField("Skill Config", _config, typeof(SkillConfig), false);
            if (_config == null) return;
        }

        EditorGUILayout.LabelField("Skill Editor", EditorStyles.boldLabel);

        _selectedType = (SkillType)EditorGUILayout.EnumPopup("Select Skill Type", _selectedType);
        _currentSkill = _config.GetSkillByType(_selectedType);
        _isNewSkill = _currentSkill == null;

        if (_isNewSkill)
        {
            EditorGUILayout.HelpBox("This skill does not exist. Fill in the fields to create a new one.", MessageType.Info);
            _currentSkill = new ActiveSkillUpgradeData { Type = _selectedType };
        }
        else
        {
            EditorGUILayout.HelpBox("Skill found. You can edit it.", MessageType.None);
        }

        DrawSkillFields();
        DrawUpgradeInfoEditor();

        if (GUILayout.Button(_isNewSkill ? "Add Skill" : "Save Skill"))
        {
            SaveSkill();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawSkillFields()
    {
        _currentSkill.SkillName = EditorGUILayout.TextField("Skill Name", _currentSkill.SkillName);
        _currentSkill.SkillIcon = (Sprite)EditorGUILayout.ObjectField("Skill Icon", _currentSkill.SkillIcon, typeof(Sprite), false);
        _currentSkill.UseType = (SkillUseType)EditorGUILayout.EnumPopup("Use Type", _currentSkill.UseType);

        if (_currentSkill is ActiveSkillUpgradeData activeSkill)
        {
            activeSkill.ActiveSkillUpgradeType = (ActiveSkillType)EditorGUILayout.EnumPopup("Active Upgrade Type", activeSkill.ActiveSkillUpgradeType);
            activeSkill.ExclusionSkillType = (SkillType)EditorGUILayout.EnumPopup("Exclusion Skill Type", activeSkill.ExclusionSkillType);
        }
        else if (_currentSkill is PassiveUpgeadeSkillData passiveSkill)
        {
            passiveSkill.ModificatorUpgradeType = (ModificatorType)EditorGUILayout.EnumPopup("Modificator Upgrade Type", passiveSkill.ModificatorUpgradeType);
        }
    }

    private void DrawUpgradeInfoEditor()
    {
        EditorGUILayout.Space();
        _showUpgrades = EditorGUILayout.Foldout(_showUpgrades, "Skill Upgrades", true);
        if (!_showUpgrades) return;

        if (_currentSkill.UpgradesInfo == null)
            _currentSkill.UpgradesInfo = new List<SkillUpgradeInfo>();

        // Синхронизация foldouts
        while (_upgradeFoldouts.Count < _currentSkill.UpgradesInfo.Count)
            _upgradeFoldouts.Add(false);
        while (_upgradeFoldouts.Count > _currentSkill.UpgradesInfo.Count)
            _upgradeFoldouts.RemoveAt(_upgradeFoldouts.Count - 1);

        for (int i = 0; i < _currentSkill.UpgradesInfo.Count; i++)
        {
            var upgrade = _currentSkill.UpgradesInfo[i];
            EditorGUILayout.BeginVertical("box");

            _upgradeFoldouts[i] = EditorGUILayout.Foldout(_upgradeFoldouts[i], $"Upgrade {i + 1} - {upgrade.Name}", true);

            if (_upgradeFoldouts[i])
            {
                upgrade.Name = EditorGUILayout.TextField("Name", upgrade.Name);
                upgrade.Level = EditorGUILayout.IntField("Level", upgrade.Level);
                upgrade.Description = EditorGUILayout.TextField("Description", upgrade.Description);
                upgrade.Value = EditorGUILayout.FloatField("Value", upgrade.Value);
                upgrade.IsPercentageValue = EditorGUILayout.Toggle("Is Percentage", upgrade.IsPercentageValue);
            }

            if (GUILayout.Button("Remove Upgrade"))
            {
                _currentSkill.UpgradesInfo.RemoveAt(i);
                _upgradeFoldouts.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Upgrade"))
        {
            _currentSkill.UpgradesInfo.Add(new SkillUpgradeInfo());
            _upgradeFoldouts.Add(false);
        }

        DrawUpgradeCopyTool();
    }

    private void DrawUpgradeCopyTool()
    {
        EditorGUILayout.Space();
        bool showCopyTool = EditorGUILayout.Foldout(true, "Upgrade Copy Tool", true);
        if (!showCopyTool) return;

        _templateUpgrade = new SkillUpgradeInfo();
        _templateUpgrade.Name = EditorGUILayout.TextField("Base Name", _templateUpgrade.Name);
        _templateUpgrade.Description = EditorGUILayout.TextField("Base Description", _templateUpgrade.Description);
        _templateUpgrade.Value = EditorGUILayout.FloatField("Base Value", _templateUpgrade.Value);
        _templateUpgrade.IsPercentageValue = EditorGUILayout.Toggle("Is Percentage", _templateUpgrade.IsPercentageValue);
        _copyCount = EditorGUILayout.IntSlider("Copy Count", _copyCount, 1, 20);

        if (GUILayout.Button("Generate Copies"))
        {
            int startLevel = _currentSkill.UpgradesInfo.Count > 0
                ? _currentSkill.UpgradesInfo.Max(u => u.Level) + 1
                : 1;

            for (int i = 0; i < _copyCount; i++)
            {
                var copy = new SkillUpgradeInfo
                {
                    Name = $"{_templateUpgrade.Name}_L{startLevel + i}",
                    Level = startLevel + i,
                    Description = _templateUpgrade.Description.Replace("{level}", (startLevel + i).ToString()),
                    Value = _templateUpgrade.Value,
                    IsPercentageValue = _templateUpgrade.IsPercentageValue
                };
                _currentSkill.UpgradesInfo.Add(copy);
                _upgradeFoldouts.Add(false);
            }
        }
    }

    private void SaveSkill()
    {
        if (_isNewSkill)
        {
            if (_currentSkill is ActiveSkillUpgradeData activeSkill)
            {
                _config.AddToActiveSkill(activeSkill);
            }
            else if (_currentSkill is PassiveUpgeadeSkillData passiveSkill)
            {
                _config.AddToPassiveSkill(passiveSkill);
            }
        }

        EditorUtility.SetDirty(_config);
        AssetDatabase.SaveAssets();
    }
}

