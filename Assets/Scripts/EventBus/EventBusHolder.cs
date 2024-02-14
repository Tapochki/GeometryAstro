using UnityEngine;
using Zenject;

namespace TandC.EventBus
{
    public class EventBusHolder : MonoBehaviour
    {
        public EventBus EventBus { get; private set; }

        public EventBusHolder() 
        {
            Debug.LogError(12);
        }

        [Inject]
        private void Construct()
        {
            EventBus = new EventBus();
        }
    }
}