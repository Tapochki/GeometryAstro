#if UNITY_ANDROID || UNITY_IOS
using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace TandC.GeometryAstro.Services
{
    public class PurchasingService : IDetailedStoreListener
    {
        private DataSystem _dataSystem;
        private SoundSystem _soundSystem;

        private List<Products> _purchasingIDs;

        private IStoreController _storeController;

        public void Construct(DataSystem dataSystem, SoundSystem soundSystem)
        {
            Utilities.Logger.Log("PurchasingService Construct", LogTypes.Info);

            _dataSystem = dataSystem;
            _soundSystem = soundSystem;
        }

        public void Initialize()
        {
            InitListOfProducts();

            ConfigurationBuilder builder = InitBuilderVariables();

            UnityPurchasing.Initialize(this, builder);
        }

        private ConfigurationBuilder InitBuilderVariables()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            for (int i = 0; i < _purchasingIDs.Count; i++)
            {
                var product = _purchasingIDs[i];
                builder.AddProduct(product.id, product.productType);
            }

            return builder;
        }

        private void InitListOfProducts()
        {
            _purchasingIDs = new List<Products>()
            {
                new Products()
                {
                    purchasingType = PurchasingType.RemoveAds,
                    productType = ProductType.NonConsumable,
                    id = "com.studio.game.removeAds",
                },
            };
        }

        public void BuyProduct(PurchasingType type)
        {
            _storeController.InitiatePurchase(GetProductInfoByType(type).id);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchase)
        {
            var product = purchase.purchasedProduct;

            switch (product.definition.id)
            {
                case "com.studio.game.removeAds":
                    RemoveAds();
                    break;

                default:
                    Utilities.Logger.Log($"Purchase with id [{product.definition.id}] not implemented", Settings.LogTypes.Error);
                    break;
            }

            Utilities.Logger.Log($"Purchase complete with id - [{product.definition.id}]", LogTypes.Info);

            return PurchaseProcessingResult.Complete;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Utilities.Logger.Log($"Initialize of purchase failed with error [{error}]", LogTypes.Error);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Utilities.Logger.Log($"Initialize of purchase failed with error [{error}] and message [{message}]", LogTypes.Error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Utilities.Logger.Log($"Purchase failed with ID [{product.definition.id}] and reason [{failureReason}]", LogTypes.Error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Utilities.Logger.Log($"Purchase failed with ID [{product.definition.id}] and failureDescription [{failureDescription}]", LogTypes.Error);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Utilities.Logger.Log($"Initialize of purchases [{controller}] and extensions [{extensions}]", LogTypes.Info);
        }

        private Products GetProductInfoByType(PurchasingType type)
        {
            for (int i = 0; i < _purchasingIDs.Count; i++)
            {
                if (_purchasingIDs[i].purchasingType == type)
                {
                    return _purchasingIDs[i];
                }
            }

            return null;
        }

        private void RemoveAds()
        {
            // TODO - add implementation
        }

        internal class Products
        {
            public PurchasingType purchasingType;
            public string id;
            public ProductType productType;
        }
    }

}
#endif