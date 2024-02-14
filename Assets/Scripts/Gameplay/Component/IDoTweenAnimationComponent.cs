using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TandC.Gameplay 
{
    public interface IDoTweenAnimationComponent
    {
        public void DoAnimation(GameObject gameObject, Action animationFinishEvent);
    }

    public class YournameDoTweenAnimation : IDoTweenAnimationComponent
    {
        public void DoAnimation(GameObject gameObject, Action animationFinishEvent)
        {
            //Do dootween animation
            // on finish
            animationFinishEvent?.Invoke();
        }
    }
}


