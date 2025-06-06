using UnityEngine;
namespace TandC.GeometryAstro.Gameplay 
{
    public interface IGameplayInputHandler
    {
        public void Init();
        public Vector2 MoveDirection { get; }
        public Vector2 RotationDirection { get; }

        public RocketInputButton RocketButton { get; }
        public SkillInputButton CloakButton { get; }
        public SkillInputButton DashButton { get; }
        public SkillInputButton LaserButton { get; }

        public void SetInteractable(bool isCanMove);
    }
}

