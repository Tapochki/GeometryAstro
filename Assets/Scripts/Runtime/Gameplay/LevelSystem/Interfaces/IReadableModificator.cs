using System;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IReadableModificator
    {
        float Value { get; }
        Action<float> OnValueChanged { get; set; }
        void SetBaseValue(float baseValue);
    }
}
