using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IItemSpawner
    {
        public void DropRandomItem(DropItemRareType type, Vector2 spawnPosition);
    }
}


