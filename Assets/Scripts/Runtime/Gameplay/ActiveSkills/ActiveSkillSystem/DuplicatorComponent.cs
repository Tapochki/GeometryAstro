using System;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class DuplicatorComponent : ITickable
    {
        private const float DUPLICATOR_TIME = 0.2f;

        private IReadableModificator _duplicateModificator;

        private Action _duplicatedAction;
        private Action _duplicatorCallback;

        private int _duplicatedCount;

        private int _maximumDuplicatorCount;

        private float _duplicatedTimer;

        private bool _duplicatorInAction;

        public DuplicatorComponent(IReadableModificator duplicateModificator, Action duplicatedAction, Action duplicatorCallback)
        {
            _duplicateModificator = duplicateModificator;
            _duplicatedAction = duplicatedAction;
            _duplicatorCallback = duplicatorCallback;

            _maximumDuplicatorCount = 0;
        }

        public void Activate() 
        {
            _duplicatedCount = 0;
            ReloadDuplicator();
        }

        public void Tick()
        {
            if(_duplicatorInAction) 
            {
                _duplicatedTimer -= Time.deltaTime;
                if(_duplicatedTimer <= 0) 
                {
                    Action();
                }
            }
        }

        private void Action() 
        {
            _duplicatorInAction = false;

            _duplicatedAction?.Invoke();
            _duplicatedCount++;

            if (_duplicatedCount >= _maximumDuplicatorCount + _duplicateModificator.Value)
                EndDuplicate();
            else
                ReloadDuplicator();
        }

        private void ReloadDuplicator() 
        {
            _duplicatedTimer = DUPLICATOR_TIME;
            _duplicatorInAction = true;
        }

        private void EndDuplicate() 
        {
            _duplicatorInAction = false;
            _duplicatorCallback?.Invoke();
        }

        public void UpgradeDuplicateCount() 
        {
            _maximumDuplicatorCount++;
        }

    }
}
