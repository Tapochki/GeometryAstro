using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.SceneSystems;
using ChebDoorStudio.Settings;

namespace ChebDoorStudio.Gameplay.Player
{
    public sealed class PlayerPickupHandler
    {
        private ItemSpawnSystem _itemSpawnSystem;
        private VaultSystem _vaultSystem;

        public PlayerPickupHandler(ItemSpawnSystem itemSpawnSystem, VaultSystem vaultSystem)
        {
            _vaultSystem = vaultSystem;

            _itemSpawnSystem = itemSpawnSystem;

            _itemSpawnSystem.OnItemPickupEvent += OnItemPickupEventHandler;
        }

        private void OnItemPickupEventHandler(ItemTypes type)
        {
            switch (type)
            {
                case ItemTypes.Coin:
                    _vaultSystem.Coins.Add(1);
                    break;

                default:
                    Utilities.Logger.NotImplementedLog(type.ToString());
                    break;
            }
        }

        public void Dispose()
        {
            _itemSpawnSystem = null;
        }
    }
}