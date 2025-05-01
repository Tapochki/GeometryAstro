using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class DroneSkill : IActiveSkill, IEventReceiver<CloakingEvent>
    {
        private const int _startDroneSpawnCount = 2;

        public UniqueId Id { get; } = new UniqueId();

        public bool IsWeapon => true;

        private ActiveSkillData _data;

        public ActiveSkillType SkillType => ActiveSkillType.Drone;

        private WeaponShootingPattern _shootingPattern;

        private IReadableModificator _sizeModificator;
        private IReadableModificator _duplicateModificator;

        private IReloadable _reloader;

        private DroneSkillView _droneSkillView;
        private GameObject _skillObject;

        private bool _isEvolved;

        private int _spawnCount;
        private bool _isStartAction;

        private float _startSpawnScale;

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<CloakingEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<CloakingEvent>);
        }

        public void Initialization()
        {
            _spawnCount = _startDroneSpawnCount;
            RegisterEvent();
        }

        public void Dispose()
        {
            UnregisterEvent();
        }

        public void OnEvent(CloakingEvent @event)
        {
            if(@event.IsActive) 
            {
                _droneSkillView.ForseStop();
            }
            else if(!@event.IsActive) 
            {
                _reloader.StartReload();
            }
        }

        public void SetObject(Transform skillParent)
        {
            _skillObject = MonoBehaviour.Instantiate(_data._skillPrefab, skillParent);
            RegisterShootingPatterns(_skillObject.transform);
        }

        private void RegisterShootingPatterns(Transform skill)
        {
            foreach (var pattern in skill.GetComponentsInChildren<WeaponShootingPattern>())
            {
                if (pattern.Type == SkillType)
                {
                    _shootingPattern = pattern;
                    break;
                }
            }
        }

        public void SetModificators(IReadableModificator sizeModificator, IReadableModificator duplicateModificator) 
        {
            _sizeModificator = sizeModificator;
            _duplicateModificator = duplicateModificator;
            _startSpawnScale = _shootingPattern.transform.localScale.y;

            _sizeModificator.OnValueChanged += UpdateSize;
            _duplicateModificator.OnValueChanged += ForceStop;

            UpdateSize(_sizeModificator.Value);
        }

        private void ForceStop(float value) 
        {
            if(_isEvolved && _isStartAction) 
            {
                _droneSkillView.ForseStop();
            }
        }

        private void UpdateSize(float value) 
        {
            _shootingPattern.transform.localScale = new Vector3(_startSpawnScale * _sizeModificator.Value, _startSpawnScale * _sizeModificator.Value, _startSpawnScale * _sizeModificator.Value);
        }

        public void SetView(Transform playerTransform,
            IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplier)
        {
            var droneObject = new GameObject("DroneSkillView");
            droneObject.transform.SetParent(playerTransform);
            droneObject.transform.localPosition = Vector3.zero;
            _droneSkillView = droneObject.AddComponent<DroneSkillView>();

            _droneSkillView.Init(_data.bulletData, damageModificator, criticalChanceModificator, criticalDamageMultiplier, _shootingPattern.Direction, EndAction);
        }

        public void SetData(ActiveSkillData data)
        {
            _data = data;
        }

        public void SetReloader(IReloadable reloader)
        {
            _reloader = reloader;
        }

        private void Action()
        {
            _isStartAction = true;
            _droneSkillView.StartDroneSpawn((int)(_spawnCount * _duplicateModificator.Value));
        }

        private void EndAction() 
        {
            _isStartAction = false;
            _reloader.StartReload();
        }

        public void Upgrade(float value = 0)
        {
            _spawnCount++;
        }

        public void Evolve()
        {
            _droneSkillView.Evolve(_data.EvolvedBulletData);
            _isEvolved = true;
        }

        public void Tick()
        {
            _reloader.Update();

            _droneSkillView.Tick();

            if (_reloader.CanAction && !_isStartAction)
            {
                Action();
            }
        }

    }
}

