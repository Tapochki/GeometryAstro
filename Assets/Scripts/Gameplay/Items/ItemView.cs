using System;
using UnityEngine;

namespace TandC.Gameplay
{
    public class ItemView : MonoBehaviour
    {
        private Action<ItemView> _onItemCollected;
        private ItemModel _itemModel;
        private IMove _moveComponent;
        private IDoTweenAnimationComponent _triggerItemDoTweenAnimationComponent;

        private Transform _playerTransform;

        private bool _isMoveToPlayer;

        private bool _isPickedByPickaper;

        private void Start()
        {
            _moveComponent = new MoveToTargetComponent(gameObject.GetComponent<Rigidbody2D>());

            _triggerItemDoTweenAnimationComponent = new TriggerItemPickupDoTweenAnimation();
        }

        private void Update()
        {
            MoveToPlayer();
        }

        public void Init(Action<ItemView> backToPoolEvent, Transform player, Sprite itemSprite, ItemModel itemModel)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = itemSprite;
            _onItemCollected = backToPoolEvent;
            _itemModel = itemModel;
            _playerTransform = player;
            _isPickedByPickaper = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ItemPickUper pickUper) && !_isPickedByPickaper)
            {
                _isPickedByPickaper = true;
                _triggerItemDoTweenAnimationComponent.DoAnimation(gameObject, _playerTransform, StartMoveToPlayer);
                return;
            }

            PickByPlayer(collision);
        }

        private void StartMoveToPlayer()
        {
            _isMoveToPlayer = true;
        }

        private void PickByPlayerFinish()
        {
            _onItemCollected?.Invoke(this);
            _itemModel.ReleseItem();

            _isPickedByPickaper = false;
        }

        private void PickByPlayer(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                PickByPlayerFinish();
            }
        }

        private void MoveToPlayer()
        {
            if (_isMoveToPlayer)
            {
                _moveComponent.Move(_playerTransform.position, 1000f);
            }
        }
    }
}