using UnityEngine;

namespace TandC.Gameplay 
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class EnemyDetector : MonoBehaviour
    {
        protected abstract void OnEnemyEnter(Enemy enemy);
        protected abstract void OnEnemyStay(Enemy enemy);
        protected abstract void OnEnemyExit(Enemy enemy);

        protected Enemy CheckIfCollisionIsEnemy(Collider2D collision) 
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                return enemy;
            }
            return null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Enemy enemy = CheckIfCollisionIsEnemy(collision);
            if(enemy != null) 
                OnEnemyEnter(enemy);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Enemy enemy = CheckIfCollisionIsEnemy(collision);
            if (enemy != null)
                OnEnemyStay(enemy);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Enemy enemy = CheckIfCollisionIsEnemy(collision);
            if (enemy != null)
                OnEnemyExit(enemy);
        }
    }
}




