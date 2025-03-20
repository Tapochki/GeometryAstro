using TandC.GeometryAstro.EventBus;
using UnityEngine;

namespace Studio
{
    public class CHESTTEST : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                EventBusHolder.EventBus.Raise(new ChestItemReleaseEvent());
            }
        }
    }
}