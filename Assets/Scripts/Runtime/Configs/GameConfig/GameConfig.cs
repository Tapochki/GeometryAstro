using UnityEngine;

namespace TandC.GeometryAstro.Data 
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "TandC/Game/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private ItemConfig _itemConfig;
        [SerializeField] private LevelConfig _phaseConfig;
        [SerializeField] private SkillConfig _skillConfig;
        [SerializeField] private ActiveSkillConfig _activeSkillConfig;
        [SerializeField] private ChanceDropItemCofig _ñhanceDropItemCofig;
        [SerializeField] private StartPlayerParams _startPlayerParams;

        public EnemyConfig EnemyConfig { get => _enemyConfig; }
        public ItemConfig ItemConfig { get => _itemConfig; }
        public ChanceDropItemCofig ChanceDropItemCofig { get => _ñhanceDropItemCofig; }
        public LevelConfig LevelsConfig { get => _phaseConfig; }
        public SkillConfig SkillConfig { get => _skillConfig; }
        public ActiveSkillConfig ActiveSkillConfig { get => _activeSkillConfig; }
        public StartPlayerParams StartPlayerParams { get => _startPlayerParams; }
    }
}

