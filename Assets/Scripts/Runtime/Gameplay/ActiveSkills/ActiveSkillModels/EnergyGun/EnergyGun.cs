using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class EnergyGun : IActiveSkill, IEventReceiver<CloakingEvent>
    {
        public UniqueId Id { get; } = new UniqueId();

        public bool IsWeapon => true;

        private ActiveSkillData _data;

        public ActiveSkillType SkillType => ActiveSkillType.EnergyGun;

        private IReloadable _reloader;

        private GameObject _weaponObject;

        private List<EnergyBeam> _activeBeams;

        private GroupCircleEnemyDetector _enemyDetector;

        private IReadableModificator _damageModificator;
        private IReadableModificator _critModificator;
        private IReadableModificator _critChanceModificator;
        private IReadableModificator _reloadModificator;

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CloakingEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CloakingEvent>);
        }

        public void OnEvent(CloakingEvent @event)
        {       
            if(@event.IsActive) 
            {
                foreach (var item in _activeBeams) 
                {
                    item.DiscardEnemy();
                }
            }
        }

        public void Dispose() 
        {
            UnregisterEvent();
        }

        public void Initialization()
        {
            RegisterEvent();
            _activeBeams = new List<EnergyBeam>();
            AddEnergyBeam();
        }

        public void SetModificators(
            IReadableModificator damageModificator,
            IReadableModificator critModificator,
            IReadableModificator critChanceModificator,
            IReadableModificator reloadModificator)
        {
            _damageModificator = damageModificator;
            _critModificator = critModificator;
            _critChanceModificator = critChanceModificator;
            _reloadModificator = reloadModificator;
        }

        public void SetEnemyDetector(GroupCircleEnemyDetector enemyDetector)
        {
            _enemyDetector = enemyDetector;
        }

        public void SetReloader(IReloadable reloader)
        {
            _reloader = reloader;
        }

        private Enemy GetClosetEnemy() 
        {
            return _enemyDetector.GetEnemy(_weaponObject.transform.position, maxDistance: _data.detectorRadius);
        }

        public void SetWeaponObject(Transform skillParent)
        {
            _weaponObject = MonoBehaviour.Instantiate(_data._skillPrefab, skillParent);
        }

        private void AddEnergyBeam() 
        {
            EnergyBeam energyBeam = GameObject.Instantiate(_data.bulletData.BulletObject, _weaponObject.transform).GetComponent<EnergyBeam>();
            energyBeam.SetReload(new SkillReloader(_data.bulletData.bulletLifeTime, _reloadModificator));
            energyBeam.SetRotation(new OnTargetRotateComponte(energyBeam.transform));
            _activeBeams.Add(energyBeam);

            energyBeam.Init(_data.bulletData, _damageModificator, _critModificator, _reloadModificator);
        }

        private void Action() 
        {
            SetNewEnemy();
            _reloader.StartReload();
        }

        private void SetNewEnemy() 
        {
            List<Enemy> allEnemies = _enemyDetector.GetEnemyGroup(_weaponObject.transform.position, _data.detectorRadius);

            HashSet<Enemy> usedEnemies = new HashSet<Enemy>();
            foreach (var beam in _activeBeams)
            {
                if (beam.IsHaveTarget && beam.TargetEnemy != null)
                    usedEnemies.Add(beam.TargetEnemy);
            }

            foreach (var beam in _activeBeams)
            {
                if (!beam.IsHaveTarget)
                {
                    Enemy target = allEnemies.FirstOrDefault(e => !usedEnemies.Contains(e) && e.IsActive);
                    if (target != null)
                    {
                        beam.SetEnemy(target);
                        usedEnemies.Add(target); 
                    }
                }
            }
        }

        public void SetData(ActiveSkillData data)
        {
            _data = data;
        }

        public void Upgrade(float value = 0)
        {
            AddEnergyBeam();
        }

        public void Evolve()
        {
            foreach(var beam in _activeBeams) 
            {
                beam.DiscardEnemy();
                beam.Evolve();
            }
        }

        public void Tick()
        {
            _reloader.Update();

            if (_reloader.CanAction) 
            {
                Action();
            }

            foreach (var beam in _activeBeams) 
            {
                beam.Tick();
            }
        }
    }
}

