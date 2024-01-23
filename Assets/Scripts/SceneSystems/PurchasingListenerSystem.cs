using System;
using TandC.ProjectSystems;
using TandC.Settings;
using UnityEngine;
using Zenject;

namespace TandC.SceneSystems
{
    public class PurchasingListenerSystem : MonoBehaviour
    {
        private PurchasingSystem _purchasingSystem;

        [Inject]
        public void Construct(PurchasingSystem purchasingSystem)
        {
            Utilities.Logger.Log("PurchasingListenerSystem Construct", LogTypes.Info);

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