using UnityEngine;
namespace TandC.GeometryAstro.Gameplay 
{
    public interface IGameplayInputHandler
    {
        public Vector2 MoveDirection { get; }
        public Vector2 RotationDirection { get; }
    }
}

