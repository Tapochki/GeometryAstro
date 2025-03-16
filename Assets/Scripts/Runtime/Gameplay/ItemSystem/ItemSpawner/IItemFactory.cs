using TandC.GeometryAstro.Settings;

namespace TandC.GeometryAstro.Gameplay 
{
    public interface IItemFactory
    {
        public ItemModel GetItemModel(ItemType type);
    }
}


