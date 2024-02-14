using UnityEngine;

public interface IInputHandler
{
    public Vector2 MoveDirection { get;  }
    public Vector2 RotationDirection { get; }
}
