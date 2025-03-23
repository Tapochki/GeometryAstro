using TandC.GeometryAstro.Data;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class AttackComponent
    {
        private const float _damageInterval = 1f; //TODO in config
        private float _timeSinceLastAttack;

        private Player _player;
        protected EnemyData _enemyData;

        private float _damageModificator;

        public AttackComponent(EnemyData enemyData, float damageModificator) 
        {
            _enemyData = enemyData;
            _damageModificator = damageModificator;
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
            Debug.LogError($" damage {_enemyData.damage} _damageModificator {_damageModificator} sum {_enemyData.damage * _damageModificator}");
            _player.TakeDamage(_enemyData.damage * _damageModificator);
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
