using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class StandardGunBuilder : ActiveSkillBuilder<StandardGun>
    {
        protected override ActiveSkillType _activeSkillType => ActiveSkillType.StandartGun;

        private int _startBulletPreloadCount = 10;


        private Transform _playerTransformSkills;


        public StandardGunBuilder(Transform playerTransformSkills)
        {
            _playerTransformSkills = playerTransformSkills;
        }

        private void SetProjectileFactory(
            IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplierModificator,
            IReadableModificator bulletSizeModificator)
        {

            _skill.SetProjectileFactory(new ProjectileFactory
                (_activeSkillData.bulletData,
                _startBulletPreloadCount,
                () => Object.Instantiate(_activeSkillData.bulletData.BulletObject).GetComponent<StandartBullet>(),
                damageModificator,
                criticalChanceModificator,
                criticalDamageMultiplierModificator, bulletSizeModificator));
        }

        private void SetDuplicatorComponent(IReadableModificator duplicatorModificator)
        {
            _skill.RegisterDuplicatorComponent(duplicatorModificator);
        }

        private void SetReloader(IReadableModificator reloadModificator)
        {
            _skill.SetReloader(new SkillReloader(_activeSkillData.shootDeley, reloadModificator));
        }

        private void SetEnemyDetector()
        {
            _skill.SetEnemyDetector(new RaycastEnemyDetector(LayerMask.GetMask("Enemy")));
        }

        private void SetSkillPrefab()
        {
            _skill.RegisterShootingPatterns(_playerTransformSkills);
        }

        protected override void ConstructSkill()
        {
            SetProjectileFactory(
                _modificatorContainer.GetModificator(ModificatorType.Damage),
                _modificatorContainer.GetModificator(ModificatorType.CriticalChance),
                _modificatorContainer.GetModificator(ModificatorType.CriticalDamageMultiplier),
                _modificatorContainer.GetModificator(ModificatorType.BulletsSize));

            SetSkillPrefab();

            SetReloader(_modificatorContainer.GetModificator(ModificatorType.ReloadTimer));

            SetEnemyDetector();

            SetDuplicatorComponent(_modificatorContainer.GetModificator(ModificatorType.Duplicator));
        }
    }
}

