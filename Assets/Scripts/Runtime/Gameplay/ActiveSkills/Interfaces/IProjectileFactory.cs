using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IProjectileFactory : ITickable
    {
        public void CreateProjectile(Vector3 position, Vector3 direction);
    }
}