using System;
using UnityEngine;

namespace TandC.Utilities
{
    [ExecuteAlways]
    public sealed class ObjectSorting : MonoBehaviour
    {
        private const float _ZStep = 1f;

        private const uint _MaxSortingOrder = 1000;

        [Tooltip("Help sort layers. Each next layer will offset by Z in minus depth")]
        public SortingLayer sortingLayer = SortingLayer.Layer1;

        [Tooltip("Help sort order in layer. Each next order will offset by Z in minus depth. Range [0:1000]")]
        [Range(0, _MaxSortingOrder)]
        public uint sortingOrder = 0;

        [Tooltip("Help sort sub order in order layer. Each next sub order will offset by Z in minus depth. Range [0:1]")]
        [Range(0f, 1f)]
        public float subSortingOrder = 0;

        private void Update()
        {
            if (!transform)
            {
                return;
            }

            if (transform.position.z != GetZOffset())
            {
                Vector3 targetPosition = transform.position;
                targetPosition.z = GetZOffset();
                transform.position = targetPosition;
            }
        }

        private float GetZOffset()
        {
            return (Enum.GetNames(typeof(SortingLayer)).Length * _MaxSortingOrder) - (((uint)sortingLayer + 1) * _MaxSortingOrder) + ((_MaxSortingOrder - sortingOrder) * _ZStep) + subSortingOrder;
        }

        public enum SortingLayer : uint
        {
            Layer1 = 0,
            Layer2,
            Layer3,
            Layer4,
            Layer5,
            Layer6,
            Layer7,
            Layer8,
            Layer9
        }
    }
}