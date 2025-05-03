using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay
{
    public class ItemView : MonoBehaviour, ITickable
    {
        private Action<ItemView> _onItemCollected;
        private ItemModel _itemModel;
        private IMove _moveComponent;
        private IDoTweenAnimationComponent _triggerItemDoTweenAnimationComponent;

        private Transform _finalTransform;

        private bool _isMoveToPlayer;

        private void Start()
        {
            _moveComponent = new MoveToTargetComponent(gameObject.GetComponent<Rigidbody2D>());

            _triggerItemDoTweenAnimationComponent = new TriggerItemPickupDoTweenAnimation();
        }

        public void Tick()
        {
            MoveToPlayer();
        }

        public void Init(Action<ItemView> backToPoolEvent, Sprite itemSprite, ItemModel itemModel)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = itemSprite;
            _onItemCollected = backToPoolEvent;
            _itemModel = itemModel;
        }

        private void StartMoveToPlayer()
        {
            _isMoveToPlayer = true;
        }

        private void PickFinish()
        {
            _onItemCollected?.Invoke(this);
            _itemModel.ReleseItem(gameObject.transform.position);
        }

        private void MoveToPlayer()
        {
            if (_isMoveToPlayer)
            {
                _moveComponent.Move(_finalTransform.position, 100f);
            }
        }

        public bool IsModelRocketAmmo() 
        {
            return _itemModel.ItemType == Settings.ItemType.RocketAmmo;
        }

        public void FirstPickUp(Transform finalTransform) 
        {
            _finalTransform = finalTransform;
            StartMoveToPlayer();
        }

        public void EndPickUp() 
        {
            if(_isMoveToPlayer)
                PickFinish();
        }
    }
}