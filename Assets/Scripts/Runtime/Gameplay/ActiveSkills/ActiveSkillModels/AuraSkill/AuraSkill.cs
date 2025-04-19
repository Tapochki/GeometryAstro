using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class AuraSkill : IActiveSkill, IEventReceiver<CloakingEvent>
    {
        public UniqueId Id { get; } = new UniqueId();

        public bool IsWeapon => true;

        private ActiveSkillData _data;

        public ActiveSkillType SkillType => ActiveSkillType.AuraGun;

        private IReloadable _reloader;

        private AuraSkillView _auraSkillView;
        private GameObject _skillObject;

        private float _rotationSpeed = 45f;

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
            RegisterEvent();
        }
        
        public void Dispose() 
        {
            UnregisterEvent();
        }

        public void OnEvent(CloakingEvent @event)
        {
            _auraSkillView.SetActive(!@event.IsActive);
        }

        public void SetObject(Transform skillParent) 
        {
            _skillObject = MonoBehaviour.Instantiate(_data._skillPrefab, skillParent);

        }

        public void SetView(IReadableModificator damageModificator,
            IReadableModificator criticalChanceModificator,
            IReadableModificator criticalDamageMultiplier,
            IReadableModificator sizeModificator) 
        {
            _auraSkillView = _skillObject.GetComponentInChildren<AuraSkillView>();
            _auraSkillView.Init(_data.bulletData, damageModificator, criticalChanceModificator, criticalDamageMultiplier, sizeModificator);
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
            _auraSkillView.ApplyDamage();
            _reloader.StartReload();
        }

        public void Upgrade(float value = 0)
        {
            Debug.LogError(value);
            _auraSkillView.Upgrade(value);
        }

        public void Evolve()
        {
            _auraSkillView.Evolve(_data.EvolvedBulletData);
        }

        public void Tick()
        {
            _reloader.Update();

            _skillObject.transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);

            if (_reloader.CanAction)
            {
                Action();
            }
        }
    }
}

