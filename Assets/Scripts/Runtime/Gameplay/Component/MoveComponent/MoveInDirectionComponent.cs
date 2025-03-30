using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class MoveInDirectionComponent : IMove
    {
        private Rigidbody2D _moveRigidbody2D;

        public MoveInDirectionComponent(Rigidbody2D moveRigidbody2D)
        {
            _moveRigidbody2D = moveRigidbody2D;
        }

        public void Move(Vector2 direction, float moveSpeed)
        {
            _moveRigidbody2D.velocity = _moveRigidbody2D.transform.up * moveSpeed * Time.deltaTime;
        }
    }
}

