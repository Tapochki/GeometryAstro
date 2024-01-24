using UnityEngine;
namespace TandC.EventBus 
{
    public readonly struct PlayerHealthChangeEvent : IEvent
    {
        public readonly float CurrentHealt;

        public PlayerHealthChangeEvent(float currentHealt)
        {
            CurrentHealt = currentHealt;
        }
    }

    public readonly struct PlayerDieEvent : IEvent
    {

    }
}

