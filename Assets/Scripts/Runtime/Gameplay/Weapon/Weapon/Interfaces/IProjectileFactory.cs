using TandC.GeometryAstro.Data;

using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IProjectileFactory
    {
        public void CreateProjectile(Vector2 position, Vector2 direction, BulletData data, float damage);
    }
}