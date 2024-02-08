using UnityEngine;

namespace TandC.Gameplay 
{
    public class LineEnemyDetector : EnemyDetector 
    {
        public bool IsEnemyOnLine { get; private set; }
        protected override void OnEnemyEnter(Enemy enemy)
        {
            IsEnemyOnLine = true;
        }

        protected override void OnEnemyStay(Enemy enemy)
        {
            IsEnemyOnLine = true;
        }

        protected override void OnEnemyExit(Enemy enemy)
        {
            IsEnemyOnLine = false;
        }
    }
}




