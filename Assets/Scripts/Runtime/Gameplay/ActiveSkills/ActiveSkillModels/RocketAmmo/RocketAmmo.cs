using UniRx;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class RocketAmmo : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<int> RocketCount => _rocketCount;

        private ReactiveProperty<int> _rocketCount = new ReactiveProperty<int>(10);

        public RocketAmmo(int startRocketCount) 
        {
            _rocketCount.Value = startRocketCount;
        }

        public bool TryShoot()
        {
            if (_rocketCount.Value > 0)
            {
                _rocketCount.Value--;
                return true;
            }
            return false;
        }

        public void AddRockets(int amount = 1)
        {
            _rocketCount.Value += amount;
        }

        public void RemoveRockets(int amount = 1)
        {
            _rocketCount.Value = Mathf.Max(0, _rocketCount.Value - amount);
        }
    }
}

