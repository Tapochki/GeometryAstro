using System;
using TandC.Data;
using TandC.Settings;
using UnityEngine;

namespace TandC.Gameplay 
{
    public class ItemView : MonoBehaviour
    {
        private Action<ItemView> _onItemCollected;
        private ItemModel _itemModel;
        private IMove _moveComponent;

        private Transform _playerTransform;

        private bool _isMoveToPlayer;

        private void Start()
        {
            _moveComponent = new MoveToTargetComponent(gameObject.GetComponent<Rigidbody2D>());
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
                _isMoveToPlayer = true;
                return;
            }
            PickByPlayer(collision);
        }

        private void PickByPlayer(Collider2D collision) 
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                _onItemCollected?.Invoke(this);
                _itemModel.ReleseItem();
            }
        }

        private void MoveToPlayer() 
        {
            if (_isMoveToPlayer) 
            {
                _moveComponent.Move(_playerTransform.position, 100f);
            }
        }
    }
}



