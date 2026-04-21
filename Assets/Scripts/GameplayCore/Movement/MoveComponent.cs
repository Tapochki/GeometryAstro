using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Moves a Rigidbody2D by setting velocity from an input direction.
    /// Used by the player ship.
    /// </summary>
    public class MoveComponent : IMove
    {
        private readonly Rigidbody2D _rigidbody;

        public MoveComponent(Rigidbody2D rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void Move(Vector2 direction, float moveSpeed)
        {
            _rigidbody.linearVelocity = direction * moveSpeed;
        }
    }
}
