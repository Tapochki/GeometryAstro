using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider2D))]
    public class EnemyBoundaryTrigger : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _boundaryOffset = 2f;

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }

            AdjustBoundarySize();
        }

        private void AdjustBoundarySize()
        {
            if (_camera == null) return;

            float height = _camera.orthographicSize;
            float width = height * _camera.aspect;

            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector2(width * _boundaryOffset, height * _boundaryOffset);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.ProccesingEnemyDeath();
            }
        }
    }
}

