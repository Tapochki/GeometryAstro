using System;

namespace TandC.GeometryAstro.Gameplay 
{
    public class Modificator : IPassiveUpgradable, IReadableModificator
    {
        public float Value => _isPercentage
            ? _baseValue * (1 + ((_upgrade + _modifier) / 100f))
            : _baseValue + _upgrade + _modifier;

        private float _baseValue;
        private float _upgrade;
        private float _modifier;
        private bool _isPercentage;

        public event Action<float> OnValueChanged;

        public Modificator(float baseValue, float upgradeValue, bool isPercentage)
        {
            _baseValue = baseValue;
            _upgrade = upgradeValue;
            _modifier = 0;
            _isPercentage = isPercentage;
        }

        public void SetBaseValue(float baseValue)
        {
            _baseValue = baseValue;
        }

        public void ApplyModifier(float value)
        {
            _modifier += value;
            OnValueChanged?.Invoke(Value);
        }
    }
}

