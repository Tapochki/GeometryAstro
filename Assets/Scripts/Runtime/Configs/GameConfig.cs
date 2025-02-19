using UnityEngine;

namespace TandC.GeometryAstro.Data 
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "TandC/Game/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private ItemConfig _itemConfig;
        [SerializeField] private PhaseConfig _phaseConfig;
        [SerializeField] private SkillConfig _skillConfig;
        [SerializeField] private WeaponConfig _weaponConfig;

        public EnemyConfig EnemyConfig { get => _enemyConfig; }
        public ItemConfig ItemConfig { get => _itemConfig; }
        public PhaseConfig PhaseConfig { get => _phaseConfig; }
        public SkillConfig SkillConfig { get => _skillConfig; }
        public WeaponConfig WeaponConfig { get => _weaponConfig; }
    }
}

