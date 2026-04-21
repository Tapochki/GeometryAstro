using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Instantly rotates a Transform to face a target position.
    /// Used by projectiles/bullets on spawn.
    /// </summary>
    public class OnTargetRotateComponent : IRotation
    {
        private readonly Transform _transform;

        public OnTargetRotateComponent(Transform transform)
        {
            _transform = transform;
        }

        public void SetRotation(Vector3 targetPosition)
        {
            Vector2 dir = (Vector2)(targetPosition - _transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            _transform.eulerAngles = new Vector3(0f, 0f, angle);
        }

        public void Update() { }
    }
}
