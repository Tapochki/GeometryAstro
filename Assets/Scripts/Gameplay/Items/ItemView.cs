using System;
using UnityEngine;

namespace TandC.Gameplay 
{
    public class ItemView : MonoBehaviour
    {
        private Action<ItemView> _onItemCollected;
        private ItemModel _itemModel;
        private IMove _moveComponent;
        private IDoTweenAnimationComponent _doTweenAnimationComponent;

        private Transform _playerTransform;

        private bool _isMoveToPlayer;

        private void Start()
        {
            _moveComponent = new MoveToTargetComponent(gameObject.GetComponent<Rigidbody2D>());
            _doTweenAnimationComponent = new YournameDoTweenAnimation();
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
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ItemPickUper pickUper))
            {
                _doTweenAnimationComponent.DoAnimation(gameObject, StartMoveToPlayer);
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
            _onItemCollected?.Invoke(this, false);
            _itemModel.ReleseItem();
        }

        private void PickByPlayer(Collider2D collision) 
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                _doTweenAnimationComponent.DoAnimation(gameObject, PickByPlayerFinish);
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



