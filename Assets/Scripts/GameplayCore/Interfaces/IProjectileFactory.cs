using System;
using UnityEngine;

namespace GameplayCore
{
    public interface IProjectileFactory : ITickable
    {
        void CreateProjectile(Vector3 position, Vector3 direction);

        /// <summary>Swap bullet data and creator after Evolve().</summary>
        void Evolve(BulletData newData, Func<BaseBullet> newCreator);
    }
}
