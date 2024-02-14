using TandC.Settings;

namespace TandC.Gameplay 
{
    public interface IItemFactory
    {
        public ItemModel GetItemModel(ItemType type);
    }
}


