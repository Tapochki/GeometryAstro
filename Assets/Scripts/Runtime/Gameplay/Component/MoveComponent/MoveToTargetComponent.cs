using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class MoveToTargetComponent : IMove
    {
        private Rigidbody2D _moveRigidbody2D;

        public MoveToTargetComponent(Rigidbody2D moveRigidbody2D)
        {
            _moveRigidbody2D = moveRigidbody2D;
        }

        public void Move(Vector2 target, float moveSpeed)
        {
            var targetPosition = target - _moveRigidbody2D.position;
            targetPosition.Normalize();
            _moveRigidbody2D.MovePosition(_moveRigidbody2D.position + (targetPosition * moveSpeed * Time.fixedDeltaTime));
        }
    }
}

