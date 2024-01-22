using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio.Utilities
{
    public class OnBehaviourHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler,
        IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Action<GameObject> OnMouseUpEvent;
        public Action<GameObject> OnMouseDownEvent;

        public Action<Collider2D> OnTrigger2DEnterEvent;
        public Action<Collider2D> OnTrigger2DStayEvent;
        public Action<Collider2D> OnTrigger2DExitEvent;

        public Action<Collider> OnTriggerEnterEvent;
        public Action<Collider> OnTriggerStayEvent;
        public Action<Collider> OnTriggerExitEvent;

        public Action<Collision2D> OnCollision2DEnterEvent;
        public Action<Collision2D> OnCollision2DStayEvent;
        public Action<Collision2D> OnCollision2DExitEvent;

        public Action<Collision> OnCollisionEnterEvent;
        public Action<Collision> OnCollisionStayEvent;
        public Action<Collision> OnCollisionExitEvent;

        public Action<PointerEventData> OnPointerEnterEvent;
        public Action<PointerEventData> OnPointerExitEvent;
        public Action<PointerEventData> OnPointerUpEvent;
        public Action<PointerEventData> OnPointerDownEvent;

        public Action<PointerEventData, GameObject> OnDragBeginEvent;
        public Action<PointerEventData, GameObject> OnDragUpdateEvent;
        public Action<PointerEventData, GameObject> OnDragEndEvent;

        public Action<string> OnAnimationStringEvent;
        public Action OnAnimationEvent;

        public void OnBeginDrag(PointerEventData eventData)
        { OnDragBeginEvent?.Invoke(eventData, gameObject); }

        public void OnDrag(PointerEventData eventData)
        { OnDragUpdateEvent?.Invoke(eventData, gameObject); }

        public void OnEndDrag(PointerEventData eventData)
        { OnDragEndEvent?.Invoke(eventData, gameObject); }

        public void OnPointerEnter(PointerEventData eventData)
        { OnPointerEnterEvent?.Invoke(eventData); }

        public void OnPointerExit(PointerEventData eventData)
        { OnPointerExitEvent?.Invoke(eventData); }

        public void OnPointerDown(PointerEventData eventData)
        { OnPointerDownEvent?.Invoke(eventData); }

        public void OnPointerUp(PointerEventData eventData)
        { OnPointerUpEvent?.Invoke(eventData); }

        private void OnMouseUp()
        { OnMouseUpEvent?.Invoke(gameObject); }

        private void OnMouseDown()
        { OnMouseDownEvent?.Invoke(gameObject); }

        private void OnTriggerEnter2D(Collider2D collider)
        { OnTrigger2DEnterEvent?.Invoke(collider); }

        private void OnTriggerStay2D(Collider2D collision)
        { OnTrigger2DStayEvent?.Invoke(collision); }

        private void OnTriggerExit2D(Collider2D collider)
        { OnTrigger2DExitEvent?.Invoke(collider); }

        private void OnTriggerEnter(Collider collider)
        { OnTriggerEnterEvent?.Invoke(collider); }

        private void OnTriggerStay(Collider collider)
        { OnTriggerStayEvent?.Invoke(collider); }

        private void OnTriggerExit(Collider collider)
        { OnTriggerExitEvent?.Invoke(collider); }

        private void OnCollisionEnter2D(Collision2D collision)
        { OnCollision2DEnterEvent?.Invoke(collision); }

        private void OnCollisionStay2D(Collision2D collision)
        { OnCollision2DStayEvent?.Invoke(collision); }

        private void OnCollisionExit2D(Collision2D collision)
        { OnCollision2DExitEvent?.Invoke(collision); }

        private void OnCollisionEnter(Collision collision)
        { OnCollisionEnterEvent?.Invoke(collision); }

        private void OnCollisionStay(Collision collision)
        { OnCollisionStayEvent?.Invoke(collision); }

        private void OnCollisionExit(Collision collision)
        { OnCollisionExitEvent?.Invoke(collision); }

        private void OnAnimationString(string parameter)
        { OnAnimationStringEvent?.Invoke(parameter); }

        private void OnAnimation()
        { OnAnimationEvent?.Invoke(); }
    }
}