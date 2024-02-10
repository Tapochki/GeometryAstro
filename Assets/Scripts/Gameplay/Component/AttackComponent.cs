using TandC.Data;
using UnityEngine;

namespace TandC.Gameplay 
{
    public class AttackComponent
    {
        private const float _damageInterval = 1f; //TODO in config
        private float _timeSinceLastAttack;

        private Player _player;
        protected EnemyData _enemyData;

        public AttackComponent(EnemyData enemyData) 
        {
            _enemyData = enemyData;
        }

        public void Update()
        {
            if(_player != null) 
            {
                _timeSinceLastAttack -= Time.deltaTime;
                if(_timeSinceLastAttack < 0) 
                {
                    ExecuteDamage();
                    ResetAttackTimer();
                }
            }
        }

        private void ResetAttackTimer() 
        {
            _timeSinceLastAttack = _damageInterval;
        }

        private void ExecuteDamage() 
        {
            _player.TakeDamage(_enemyData.damage);
        }

        public void SubscribePlayer(Player player)
        {
            _player = player;
            ExecuteDamage();
            ResetAttackTimer();
        }

        public void UnSubscribePlayer()
        {
            _player = null;
        }
    }
}
