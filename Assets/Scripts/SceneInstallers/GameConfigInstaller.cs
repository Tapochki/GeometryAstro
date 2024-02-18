using UnityEngine;
using Zenject;

namespace TandC.Data
{
    [CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "TandC/Installers/GameConfigInstaller")]
    public class GameConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private GameConfig gameConfig;

        public override void InstallBindings()
        {
            Container.Bind<EnemyConfig>().FromInstance(gameConfig.EnemyConfig);
            Container.Bind<ItemConfig>().FromInstance(gameConfig.ItemConfig);
            Container.Bind<PhaseConfig>().FromInstance(gameConfig.PhaseConfig);
            Container.Bind<SkillConfig>().FromInstance(gameConfig.SkillConfig);
            Container.Bind<WeaponConfig>().FromInstance(gameConfig.WeaponConfig);
        }
    }
}

