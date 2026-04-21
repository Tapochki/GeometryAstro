using UnityEngine;

namespace GameplayCore
{
    public interface IMove
    {
        void Move(Vector2 direction, float moveSpeed);
    }
}
