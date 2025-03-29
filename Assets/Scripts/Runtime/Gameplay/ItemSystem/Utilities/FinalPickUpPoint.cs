using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class FinalPickUpPoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ItemView itemView))
            {
                itemView.EndPickUp();
                return;
            }
        }
    }
}

