using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

public class TestSkill : MonoBehaviour
{
    [SerializeField]
    private ModificatorType _selectedType;

    [SerializeField]
    private float SkillValue;

    [SerializeField]
    private int HealthRestoreValue;

    [SerializeField]
    private int ExpAddValue;

    [SerializeField]
    private float FreezeTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            TestUpgradeSkill();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            FreezeAll();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenChest();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddEXP();
        }
    }

    private void FreezeAll()
    {
        EventBusHolder.EventBus.Raise(new FrozeBombReleaseEvent(FreezeTime));
    }

    private void OpenChest()
    {
        EventBusHolder.EventBus.Raise(new ChestItemReleaseEvent());
    }

    private void AddEXP() 
    {
        EventBusHolder.EventBus.Raise(new ExpirienceItemReleaseEvent(ExpAddValue));
    }

    private void Heal()
    {
        EventBusHolder.EventBus.Raise(new PlayerHealReleaseEvent(HealthRestoreValue));
    }

    [ContextMenu("Test Upgrade Skill")]
    private void TestUpgradeSkill() 
    {
        Debug.LogError($"TestUpgradeSkill Type {_selectedType} on Value {SkillValue}"); ;
        EventBusHolder.EventBus.Raise(new PassiveSkillUpgradeEvent(_selectedType, SkillValue));
    }
}
