using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ExplosiveBullet : BaseBullet
    {
        [SerializeField] private GameObject _explosionPrefab;

        protected override void BulletHit()
        {
            //Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Dispose();
        }
    }
}


