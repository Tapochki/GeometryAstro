using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace Studio
{
    public class CHESTTEST : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                EventBusHolder.EventBus.Raise(new ChestItemReleaseEvent());
            }
        }
    }
}