using UnityEngine;

namespace TandC.Gameplay
{
    public interface IMove
    {
        public void Move(Vector2 direction, float moveSpeed);
    }

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

    public class MoveInDirectionComponent : IMove
    {
        private Rigidbody2D _moveRigidbody2D;

        public MoveInDirectionComponent(Rigidbody2D moveRigidbody2D)
        {
            _moveRigidbody2D = moveRigidbody2D;
        }

        public void Move(Vector2 direction, float moveSpeed)
        {
            _moveRigidbody2D.AddForce(_moveRigidbody2D.transform.up * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}