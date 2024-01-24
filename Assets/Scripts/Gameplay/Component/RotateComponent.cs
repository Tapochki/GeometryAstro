using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TandC.Gameplay 
{
    public class RotateComponent
    {
        private const float _rotationSpeed = 1000f;
        private Transform _transform;
        private Quaternion _lastRotation;

        public RotateComponent(Transform transform)
        {
            _transform = transform;
            _lastRotation = _transform.rotation;
        }

        public void RotateTowards(Vector2 direction)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, toRotation, Time.deltaTime * _rotationSpeed);
            _lastRotation = _transform.rotation;
        }

        public void SaveLastRotation()
        {
            _transform.rotation = _lastRotation;
        }
    }
}

