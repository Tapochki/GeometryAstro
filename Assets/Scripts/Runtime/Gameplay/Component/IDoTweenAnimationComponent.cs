using DG.Tweening;
using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace TandC.GeometryAstro.Gameplay
{
    public interface IDoTweenAnimationComponent
    {
        public void DoAnimation(GameObject gameObject, Transform target, Action animationFinishEvent);
    }

    public class TriggerItemPickupDoTweenAnimation : IDoTweenAnimationComponent
    {
        public void DoAnimation(GameObject gameObject, Transform target, Action animationFinishEvent)
        {
            Vector2 initialDragDirection = ((Vector2)target.position - (Vector2)gameObject.transform.position).normalized;

            gameObject.transform.DOLocalMove((Vector2)gameObject.transform.position + initialDragDirection, 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                animationFinishEvent?.Invoke();
            });
        }
    }
}