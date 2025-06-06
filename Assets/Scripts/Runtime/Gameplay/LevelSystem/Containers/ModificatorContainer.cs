using System.Collections.Generic;
using System;
using TandC.GeometryAstro.Settings;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Services;
using VContainer;
using System.Linq;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ModificatorContainer : IEventReceiver<PassiveSkillUpgradeEvent>
    {
        private Dictionary<ModificatorType, IPassiveUpgradable> _modificators = new();

        private StartPlayerParams _startPlayerParamConfig;
        private DataService _dataService;

        public UniqueId Id { get; } = new UniqueId();

        [Inject]
        private void Construct(GameConfig gameConfig, DataService dataService) 
        {
            _startPlayerParamConfig = gameConfig.StartPlayerParams;
            _dataService = dataService;
        }

        private void RegisterEvent()
        {
            EventBusHolder.EventBus.Register(this as IEventReceiver<PassiveSkillUpgradeEvent>);
        }

        private void UnregisterEvent()
        {
            EventBusHolder.EventBus.Unregister(this as IEventReceiver<PassiveSkillUpgradeEvent>);
        }


        public void Init()
        {
            foreach (var param in _startPlayerParamConfig.StartParams)
            {
                float upgradeValue = _dataService.ModificatorUpgrade.UpgradeModificatorsData
                    .FirstOrDefault(x => x.IncreamentData.Type == param.Type)?.CurrentValue ?? 0f;

                _modificators[param.Type] = new Modificator(param.StartValue, upgradeValue, param.IsPercentageValue);
            }
            RegisterEvent();
        }

        public void OnEvent(PassiveSkillUpgradeEvent @event)
        {
            ApplyModificator(@event.PassiveSkillType, @event.UpgradeValue);
        }

        private void ApplyModificator(ModificatorType type, float value)
        {
            if (_modificators.TryGetValue(type, out var modificator))
            {
                modificator.ApplyModifier(value);
            }
        }

        public IReadableModificator GetModificator(ModificatorType type)
        {
            return _modificators.TryGetValue(type, out var modificator) ? modificator as IReadableModificator : null;
        }

        private void Dispose() 
        {
            UnregisterEvent();
        }
    }

}
