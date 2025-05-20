using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.EventBus;
using TandC.GeometryAstro.Services;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public class DamageVFXContainer : BaseEffectContainer<CreateDamageEffect, DamageEffect>
    {
        private readonly DataService _dataService;

        public DamageVFXContainer(EffectsConfig effectsConfig, DataService dataService) : base(effectsConfig)
        {
            _dataService = dataService;
        }

        protected override BaseEffectConfig GetConfig(EffectsConfig effectsConfig)
        {
            return effectsConfig.DamageEffectConfig;
        }

        protected override string GetContainerName()
        {
            return "DAMAGE_VFX";
        }

        public override void OnEvent(CreateDamageEffect @event)
        {
            bool isVisible = @event.IsCrit ? _dataService.AppSettingsData._isShowCritDamageEffect
                                          : _dataService.AppSettingsData._isShowDamageEffect;

            if (!isVisible)
                return;

            Color color = @event.IsCrit ? ((DamageEffectConfig)_baseConfig).CritColor : ((DamageEffectConfig)_baseConfig).StandartColor;

            DamageEffect effect = _effectPool.Get();
            effect.StartEffect(@event.Position, (int)@event.Damage, color);
        }
    }
}