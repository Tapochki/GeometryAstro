using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RifleGunBuilder : ActiveSkillBuilder<RifleGun>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.RifleGun;

        private Transform _playerTransformSkills;

        private int _startBulletPreloadCount = 20;

        public RifleGunBuilder(Transform playerTransformSkills) 
        {
            _playerTransformSkills = playerTransformSkills;
        }

        private void SetProjectileFactory(IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplierModificator,
            IReadableModificator bulletSizeModificator)
        {
            _skill.SetProjectileFactory(new ProjectileFactory(
                _activeSkillData.bulletData, 
                _startBulletPreloadCount,
                () => Object.Instantiate(_activeSkillData.bulletData.BulletObject).GetComponent<StandartBullet>(),
                damageModificator, 
                criticalChanceModificator, 
                criticalDamageMultiplierModificator, 
                bulletSizeModificator));
        }

        private void SetDuplicatorComponent(IReadableModificator duplicatorModificator)
        {
            _skill.RegisterDuplicatorComponent(duplicatorModificator);
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            _skill.SetReloader(new SkillReloader(_activeSkillData.shootDeley, reloadModificator));
        }

        private void SetSkillPrefab()
        {
            _skill.RegisterShootingPatterns(_playerTransformSkills);
        }

        private void SetStartShootCountPrefab()
        {
            _skill.SetStartShotsPerCycle(_config.AdditionalSkillConfig.RifleConfig.StartShootCount);
        }

        protected override void ConstructSkill()
        {
            SetProjectileFactory(
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier),
                _modificatorContainer.GetModificator(ModificatorType.BulletsSize));

            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));
            SetStartShootCountPrefab();
            SetSkillPrefab();

            SetDuplicatorComponent(_modificatorContainer.GetModificator(ModificatorType.Duplicator));
        }
    }
}

