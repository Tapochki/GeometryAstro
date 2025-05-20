using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay.VFX 
{
    public abstract class Effect : MonoBehaviour, IEffect
    {
        protected Action<IEffect> _returnToPoolAction;

        public virtual void Init(Action<IEffect> returnToPoolAction)
        {
            _returnToPoolAction = returnToPoolAction;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Dispose()
        {
            _returnToPoolAction = null;
            Destroy(gameObject);
        }
    }
}

