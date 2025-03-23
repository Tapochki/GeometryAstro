using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

public class TestSkill : MonoBehaviour
{
    [SerializeField]
    private ModificatorType _selectedType;

    [SerializeField]
    private float Value;

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
    }

    private void Heal()
    {
        EventBusHolder.EventBus.Raise(new PlayerHealReleaseEvent(100));
    }

    [ContextMenu("Test Upgrade Skill")]
    private void TestUpgradeSkill() 
    {
        Debug.LogError($"TestUpgradeSkill Type {_selectedType} on Value {Value}"); ;
        EventBusHolder.EventBus.Raise(new PassiveSkillUpgradeEvent(_selectedType, Value));
    }
}
