using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Moves a Rigidbody2D forward along its own up-axis at a fixed speed.
    /// Used by projectiles/bullets.
    /// </summary>
    public class MoveInDirectionComponent : IMove
    {
        private readonly Rigidbody2D _rigidbody;

        public MoveInDirectionComponent(Rigidbody2D rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void Move(Vector2 direction, float moveSpeed)
        {
            _rigidbody.linearVelocity = _rigidbody.transform.up * moveSpeed * Time.deltaTime;
        }
    }
}
