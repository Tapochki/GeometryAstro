using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "MenuConfig", menuName = "TandC/Menu/MenuConfig", order = 1)]
    public class MenuConfig : ScriptableObject
    {
        [SerializeField] private ModificatorUpgradeConfig _modificatorConfig;

        public ModificatorUpgradeConfig ModificatorConfig { get => _modificatorConfig; }
    }
}
