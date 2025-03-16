using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public abstract class ItemModel
    {
        private readonly Sprite _itemSprite;

        public Sprite ItemSprite { get => _itemSprite; }

        protected ItemModel(Sprite itemSprite)
        {
            _itemSprite = itemSprite ?? throw new System.ArgumentNullException(nameof(itemSprite), "Sprite cannot be null");
        }

        public abstract void ReleseItem();
    }
}

