using TandC.Gameplay;
using UnityEngine;
using Zenject;

namespace TandC.EventBus
{
    public class EventBusHolder : MonoBehaviour
    {
        public EventBus EventBus { get; private set; }

        [Inject]
        private void Construct()
        {
            EventBus = new EventBus();
        }
    }
}