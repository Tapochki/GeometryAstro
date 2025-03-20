using System;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IReadableModificator
    {
        float Value { get; }
        event Action<float> OnValueChanged;
        void SetBaseValue(float baseValue);
    }
}
