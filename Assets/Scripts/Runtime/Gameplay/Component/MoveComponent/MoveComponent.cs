using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class MoveComponent : IMove
    {
        private Rigidbody2D _moveRigidbody2D;

        public MoveComponent(Rigidbody2D moveRigidbody2D)
        {
            _moveRigidbody2D = moveRigidbody2D;
        }

        public void Move(Vector2 direction, float moveSpeed)
        {
            Vector2 moveVelocity = direction * moveSpeed;
            _moveRigidbody2D.velocity = moveVelocity;
        }
    }
}

