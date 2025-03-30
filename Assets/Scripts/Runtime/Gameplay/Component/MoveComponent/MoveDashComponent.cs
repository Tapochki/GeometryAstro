using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class MoveDashComponent : IMove, IDashMove
    {
        private readonly Rigidbody2D _moveRigidbody2D;

        private readonly Collider2D _collider2D;

        private readonly IMove _baseMove;

        private readonly IReadableModificator _dashModificator;

        private bool _isDash;

        public MoveDashComponent(Rigidbody2D moveRigidbody2D, Collider2D collider, IReadableModificator dashModificator, IMove baseMove)
        {
            _baseMove = baseMove;
            _moveRigidbody2D = moveRigidbody2D;
            _collider2D = collider;
            _dashModificator = dashModificator;
        }

        public void Move(Vector2 direction, float moveSpeed)
        {
            if (_isDash)
                Dash(direction, moveSpeed);
            else
                NormalMove(direction, moveSpeed);
        }

        private void NormalMove(Vector2 direction, float moveSpeed)
        {
            _baseMove.Move(direction, moveSpeed);
        }

        private void Dash(Vector2 direction, float moveSpeed)
        {
            _isDash = true;
            _moveRigidbody2D.velocity = direction * moveSpeed * _dashModificator.Value;
        }

        public void StartDash()
        {
            _collider2D.enabled = false;
            _isDash = true;
        }

        public void StopDash()
        {
            _collider2D.enabled = true;
            _isDash = false;
        }
    }
}
