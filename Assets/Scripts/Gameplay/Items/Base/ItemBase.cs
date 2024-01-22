using ChebDoorStudio.Settings;
using System;
using UnityEngine;

namespace ChebDoorStudio.Gameplay.Items.Base
{
    public class ItemBase : MonoBehaviour
    {
        public event Action<ItemBase> OnItemPickupEvent;

        public ItemTypes ItemType { get; private set; }

        public virtual void Initialize()
        {
        }

        public virtual void Pickup()
        {
            OnItemPickupEvent?.Invoke(this);
        }

        public virtual void Dispose()
        {
        }
    }
}