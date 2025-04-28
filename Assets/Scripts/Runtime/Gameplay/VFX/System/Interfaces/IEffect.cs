using System;

namespace TandC.GeometryAstro.Gameplay.VFX
{
    public interface IEffect
    {
        void Init(Action<IEffect> returnToPoolAction);
        void Show();
        void Hide();
        void Dispose();
    }
}