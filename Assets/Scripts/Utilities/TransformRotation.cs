using UnityEngine;

namespace ChebDoorStudio.Utilities
{
    [ExecuteAlways]
    public class TransformRotation : MonoBehaviour
    {
        public Vector3 rotation = Vector3.zero;

        public bool rotate;

        private void FixedUpdate()
        {
            if (rotate && transform != null)
            {
                transform.localEulerAngles += rotation * Time.fixedDeltaTime;
            }
        }
    }
}