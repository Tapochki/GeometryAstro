using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TandC.Gameplay 
{
    public class MoveComponent 
    {
        private Rigidbody2D _moveRigidbody2D;
        public MoveComponent(Rigidbody2D moveRigidbody2D) 
        {
            _moveRigidbody2D = moveRigidbody2D;
        }

        public void Move(Vector2 direction, float moveSpeed)
        {
            Vector2 moveVelocity = direction * moveSpeed;
            _moveRigidbody2D.velocity = moveVelocity;
            Debug.LogError(moveVelocity);
        }
    }
}

