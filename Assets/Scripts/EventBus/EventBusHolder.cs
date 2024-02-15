using UnityEngine;
using Zenject;

namespace TandC.EventBus
{
    public class EventBusHolder : MonoBehaviour
    {
        public EventBus EventBus { get; private set; }

        public EventBusHolder() 
        {
            EventBus = new EventBus();
        }
    }
}