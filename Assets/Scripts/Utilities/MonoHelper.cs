using System;
using UnityEngine;

namespace ChebDoorStudio.Utilities
{
    public class MonoHelper : MonoBehaviour
    {
        public event Action OnUpdateEvent;

        public event Action OnFixedUpdateEvent;

        public event Action OnLateUpdateEvent;

        private void Update()
        {
            OnUpdateEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdateEvent?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdateEvent?.Invoke();
        }
    }
}