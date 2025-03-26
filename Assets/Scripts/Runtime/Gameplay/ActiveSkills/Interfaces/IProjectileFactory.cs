using System;
using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IProjectileFactory : ITickable
    {
        public void Evolve(BulletData newEvolveData, Func<BaseBullet> newBulletCreator);
        public void CreateProjectile(Vector3 position, Vector3 direction);
    }
}