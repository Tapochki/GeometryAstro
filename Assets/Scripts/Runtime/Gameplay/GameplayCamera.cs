using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{
    public class GameplayCamera : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _back_0Material;
        [SerializeField] private MeshRenderer _back_1Material;
        [SerializeField] private MeshRenderer _back_2Material;

        public Camera Camera { get; private set; }

        [Inject]
        private Player _player;

        private Vector2 PlayerPosition => _player.transform.position;

        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            UpdateParallax();
        }

        private void UpdatePosition()
        {
            transform.position = PlayerPosition;
        }

        private void UpdateParallax()
        {
            UpdateMaterialOffset(_back_0Material, 30f);
            UpdateMaterialOffset(_back_1Material, 20f);
            UpdateMaterialOffset(_back_2Material, 16f);
        }

        private void UpdateMaterialOffset(MeshRenderer meshRenderer, float speed)
        {
            meshRenderer.material.mainTextureOffset = PlayerPosition / speed * Time.deltaTime;
        }
    }
}