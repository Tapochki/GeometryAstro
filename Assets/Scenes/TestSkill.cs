using System.Collections.Generic;
using TandC.GeometryAstro.Gameplay;
using TandC.GeometryAstro.Settings;
using UnityEngine;
using static TandC.GeometryAstro.Gameplay.SkillService;

public class TestSkill : MonoBehaviour
{
    [SerializeField] SkillService _skillService;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            TestSkillServiceUpgradeData();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            TestSkillServiceChestData();
        }
    }

    private void TestSkillServiceUpgradeData() 
    {
        List<PreparationSkillData> preparationSkillDatas = _skillService.GetUpgradeOptions(false);

        foreach(var preparationSkillData in preparationSkillDatas) 
        {
            string activationColor = preparationSkillData.SkillActivationType switch
            {
                SkillActivationType.NewSkill => "yellow",
                SkillActivationType.UpgradeActive => "red",
                SkillActivationType.UpgradePassive => "blue",
                SkillActivationType.Evolution => "green",
                _ => "white"                      
            };

            Debug.LogError($"<color={activationColor}>{preparationSkillData.SkillActivationType}</color>" +
           $"\nName {preparationSkillData.SkillUpgradeInfo.Name}" +
           $"\nDescription {preparationSkillData.SkillUpgradeInfo.Description}" +
           $"\nLevel {preparationSkillData.SkillUpgradeInfo.Level}" +
           $"\nValue {preparationSkillData.SkillUpgradeInfo.Value}" +
           $"\nSkillType {preparationSkillData.SkillType}");
        }
        var randomIndex = UnityEngine.Random.Range(0, preparationSkillDatas.Count);
        var selectedSkillData = preparationSkillDatas[randomIndex];

        Debug.LogError($"\nApply {selectedSkillData.SkillType}");
        selectedSkillData.ActivateSkillAction.Invoke(selectedSkillData.SkillType, selectedSkillData.SkillActivationType);
    }

    private void TestSkillServiceChestData()
    {
        List<PreparationSkillData> preparationSkillDatas = _skillService.GetUpgradeOptions(false);

        foreach (var preparationSkillData in preparationSkillDatas)
        {
            Debug.LogError($"Name {preparationSkillData.SkillUpgradeInfo.Name}" +
                $"\nDescription {preparationSkillData.SkillUpgradeInfo.Description}" +
                $"\n Level {preparationSkillData.SkillUpgradeInfo.Level}" +
                $"\nValue {preparationSkillData.SkillUpgradeInfo.Value}" +
                $"\nSkillType {preparationSkillData.SkillType}" +
                $"\nSkillActivationType {preparationSkillData.SkillActivationType}"); ;
        }
    }
}
