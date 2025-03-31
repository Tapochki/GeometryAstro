using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class MachineGun : IActiveSkill
    {
        public bool IsWeapon { get => true; }

        private IProjectileFactory _projectileFactory;
        private IReloadable _reloader;
        private ActiveSkillData _data;

        private WeaponShootingPattern _weaponShootingPattern;

        private int _shotsPerCycle;
        private int _shotsFired;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.MachineGun;

        private DuplicatorComponent _duplicatorComponent;

        private bool _shootStart;

        public void SetData(ActiveSkillData data) 
        {
            _data = data;
        }

        public void SetProjectileFactory(IProjectileFactory projectileFactory) 
        {
            _projectileFactory = projectileFactory;
        }

        public void SetReloader(IReloadable reloader)
        {
            _reloader = reloader;
        }

        public void Initialization() 
        {
            _reloader.StartReload();
        }

        public void RegisterDuplicatorComponent(IReadableModificator duplicateModificator)
        {
            _duplicatorComponent = new DuplicatorComponent(duplicateModificator, TryShoot, EndShoot);
        }

        public void RegisterShootingPatterns(Transform skillParent)
        {
            GameObject skillObject = MonoBehaviour.Instantiate(_data._skillPrefab, skillParent);

            foreach (var pattern in skillObject.GetComponentsInChildren<WeaponShootingPattern>())
            {
                if (pattern.Type == SkillType)
                {
                    _weaponShootingPattern = pattern;
                    break;
                }
            }
        }

        private void ShootAction() 
        {
            _shootStart = true;
            _shotsFired = 0;
        }

        private void TryShoot()
        {
            if (!_shootStart || _shotsFired >= _shotsPerCycle)
                return;

            Vector2 origin = _weaponShootingPattern.Origin.position;
            Vector2 baseDirection = (_weaponShootingPattern.Direction.position - _weaponShootingPattern.Origin.position).normalized;

            float spread = UnityEngine.Random.Range(-5f, 5f);
            Vector2 direction = Quaternion.Euler(0, 0, spread) * baseDirection;

            Shoot(origin, direction);

            _shotsFired++;
            if (_shotsFired >= _shotsPerCycle)
            {
                EndShoot();
            }
        }

        private void EndShoot() 
        {
            _shootStart = false;
            _reloader.StartReload();
        }

        private void Shoot(Vector2 origin, Vector2 direction)
        {
            _projectileFactory.CreateProjectile(
                origin,
                direction
            );
        }

        public void Upgrade(float value = 0)
        {
            _shotsPerCycle += (int)value;
        }

        public void Evolve()
        {
            _projectileFactory.Evolve(_data.EvolvedBulletData, () => Object.Instantiate(_data.EvolvedBulletData.BulletObject).GetComponent<BulletWithHealth>());
        }

        public void Tick()
        {
            _duplicatorComponent?.Tick();
            _reloader.Update();
            _projectileFactory.Tick();
            if (_reloader.CanAction)
            {
                ShootAction();
            }

            TryShoot();
        }
    }
}
