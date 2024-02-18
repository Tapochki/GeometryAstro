using TandC.Settings;
using UnityEngine;

namespace TandC.Gameplay 
{
    public interface IItemSpawner
    {
        public void DropRandomItem(DropItemRareType type, Vector2 spawnPosition);
    }
}


