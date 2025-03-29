using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class RocketGunBuilder : ActiveSkillBuilder<RocketGun>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.RocketGun;

        private int _startBulletPreloadCount = 2;

        private readonly RocketInputButton _rocketButton;

        private Player _player;
        private IItemSpawner _itemSpawner;

        public RocketGunBuilder(Player player, IItemSpawner itemSpawner, RocketInputButton rocketButton)
        {
            _player = player;
            _itemSpawner = itemSpawner;
            _rocketButton = rocketButton;
        }

        private void SetProjectileFactory(IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplierModificator,
            IReadableModificator bulletSizeModificator)
        {
            _skill.SetProjectileFactory(damageModificator, criticalChanceModificator, criticalDamageMultiplierModificator, bulletSizeModificator, _startBulletPreloadCount);
        }

        private void SetDuplicatorComponent(IReadableModificator duplicatorModificator)
        {
            _skill.RegisterDuplicatorComponent(duplicatorModificator);
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            _skill.SetReloader(new SkillReloader(_activeSkillData.shootDeley, reloadModificator), _rocketButton);
        }

        private void SetSkillPrefab()
        {
            _skill.RegisterShootingPatterns(_player.SkillTransform);
        }

        private void SetAreaDamageInterval() 
        {
            _skill.SetAreaDamageInterval(_config.AdditionalSkillConfig.AreaEffectConfig.damageInterval);
        }

        private void CreateRocketAmmoContainer() 
        {
            RocketAmmo rocketAmmo = new RocketAmmo(_config.AdditionalSkillConfig.RocketConfig.StartRocketCount);
            _player.SetRocketAmmo(rocketAmmo.RocketCount, rocketAmmo.MaxRocketCount);
            _itemSpawner.SetCanSpawnRocket();
            _skill.InitRocketAmmo(rocketAmmo);
        }

        protected override void ConstructWeapon()
        {
            SetAreaDamageInterval();

            SetProjectileFactory(
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier),
                _modificatorContainer.GetModificator(ModificatorType.BulletsSize));

            CreateRocketAmmoContainer();

            SetSkillPrefab();

            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));

            SetDuplicatorComponent(_modificatorContainer.GetModificator(ModificatorType.Duplicator));
        }
    }
}

