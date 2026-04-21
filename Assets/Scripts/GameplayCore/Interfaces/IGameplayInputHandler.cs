using UnityEngine;

namespace GameplayCore
{
    public interface IGameplayInputHandler
    {
        Vector2 MoveDirection { get; }
        Vector2 RotationDirection { get; }
    }
}
