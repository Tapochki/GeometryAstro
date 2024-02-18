using UnityEngine;

public interface IGameplayInputHandler
{
    public Vector2 MoveDirection { get;  }
    public Vector2 RotationDirection { get; }
}
