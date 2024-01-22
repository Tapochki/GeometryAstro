using ChebDoorStudio.ProjectSystems;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ChebDoorStudio.UI.Views.Base
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class View : MonoBehaviour
    {
        private GameObject _selfObject;
        private Canvas _canvas;

        protected Scenes.Base.SceneView _sceneView;
        protected SoundSystem _soundSystem;

        [Inject]
        public void Construct(SoundSystem soundSystem)
        {
            _soundSystem = soundSystem;
        }

        public void ForceActiveGameObject() => _selfObject.SetActive(true);

        public virtual void Initialize()
        {
            _sceneView = transform.GetComponentInParent<Scenes.Base.SceneView>();

            _selfObject = this.gameObject;

            if (!_selfObject.activeInHierarchy)
            {
                ForceActiveGameObject();
            }

            _canvas = GetComponent<Canvas>();
            _canvas.pixelPerfect = true;
            _canvas.vertexColorAlwaysGammaSpace = true;
        }

        public virtual void Show()
        {
            if (_canvas.enabled)
            {
                return;
            }

            if (_canvas == null)
            {
                return;
            }

            _canvas.enabled = true;
        }

        public virtual void Hide()
        {
            if (!_canvas.enabled)
            {
                return;
            }

            if (_canvas == null)
            {
                return;
            }

            _canvas.enabled = false;
        }

        public virtual void Dispose()
        {
            _canvas = null;
            _selfObject = null;

            _soundSystem = null;
        }

        public virtual void Update()
        {
        }
    }
}