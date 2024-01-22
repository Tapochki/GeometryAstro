using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio.Settings
{
    public class EventBus
    {
        #region Default Unity Events

        public static Action<GameObject> OnMouseUpEvent;
        public static Action<GameObject> OnMouseDownEvent;

        public static Action<Collider2D> OnTrigger2DEnterEvent;
        public static Action<Collider2D> OnTrigger2DStayEvent;
        public static Action<Collider2D> OnTrigger2DExitEvent;

        public static Action<Collider> OnTriggerEnterEvent;
        public static Action<Collider> OnTriggerStayEvent;
        public static Action<Collider> OnTriggerExitEvent;

        public static Action<Collision2D> OnCollision2DEnterEvent;
        public static Action<Collision2D> OnCollision2DStayEvent;
        public static Action<Collision2D> OnCollision2DExitEvent;

        public static Action<Collision> OnCollisionEnterEvent;
        public static Action<Collision> OnCollisionStayEvent;
        public static Action<Collision> OnCollisionExitEvent;

        public static Action<PointerEventData> OnPointerEnterEvent;
        public static Action<PointerEventData> OnPointerExitEvent;
        public static Action<PointerEventData> OnPointerUpEvent;
        public static Action<PointerEventData> OnPointerDownEvent;

        public static Action<PointerEventData, GameObject> OnDragBeginEvent;
        public static Action<PointerEventData, GameObject> OnDragUpdateEvent;
        public static Action<PointerEventData, GameObject> OnDragEndEvent;

        #endregion Default Unity Events

        #region App Events

        public static Action OnCacheLoadedEvent;
        public static Action<CacheType> OnCacheResetEvent;

        public static Action<SceneNames> OnSceneLoadedEvent;
        public static Action<float> OnSceneLoadingProgressEvent;

        public static Action<Languages> OnLanguageWasChangedEvent;

        public static Action<GameStates> OnGameStateWasChangedEvent;

        public static Action OnSystemsBindedEvent;

        public static Action OnAdvertismentCompleteEvent;
        public static Action OnAdvertismentFailedEvent;

        #endregion App Events

        #region Input Events

        public static Action OnEscapeButtonDownEvent;

        #endregion Input Events

        #region Animation Events

        public static Action<string> OnAnimationStringEvent;
        public static Action OnAnimationEvent;

        #endregion Animation Events

        #region Gameplay Events

        public static Action OnGameplayStartedEvent;
        public static Action OnGameplayStopedEvent;
        public static Action<bool> OnGameplayPausedEvent;

        #endregion Gameplay Events
    }
}