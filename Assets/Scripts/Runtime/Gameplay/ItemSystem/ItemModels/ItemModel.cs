using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public abstract class ItemModel
    {
        private readonly Sprite _itemSprite;

        public Sprite ItemSprite { get => _itemSprite; }

        public ItemType ItemType { get; private set; }

        protected ItemModel(Sprite itemSprite, ItemType type)
        {
            ItemType = type;
            _itemSprite = itemSprite ?? throw new System.ArgumentNullException(nameof(itemSprite), "Sprite cannot be null");
        }

        public abstract void ReleseItem(Vector3 position);
    }
}

