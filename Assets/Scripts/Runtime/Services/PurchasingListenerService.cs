#if UNITY_ANDROID || UNITY_IOS
using System;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Services
{
    public class PurchasingListenerService : MonoBehaviour
    {
        private PurchasingSystem _purchasingSystem;

        public void Construct(PurchasingSystem purchasingSystem)
        {
            Utilities.Logger.Log("PurchasingListenerService Construct", LogTypes.Info);

            _purchasingSystem = purchasingSystem;
        }

        public void BuyProduct(string subType)
        {
            if (subType == "Unknown")
            {
                Utilities.Logger.Log("Unknown is not implemented", LogTypes.Error);
                return;
            }

            if (Enum.TryParse(subType, out PurchasingType type))
            {
                _purchasingSystem.BuyProduct(type);
            }
            else
            {
                Utilities.Logger.Log($"Can't parse [{subType}] to [{type}]", LogTypes.Error);
            }
        }
    }
}
#endif