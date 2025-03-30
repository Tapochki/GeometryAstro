using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IMove
    {
        public void Move(Vector2 direction, float moveSpeed);
    }
}