using UnityEngine;

namespace GameplayCore
{
    public interface IRotation
    {
        void SetRotation(Vector3 targetDirection);
        void Update();
    }
}
