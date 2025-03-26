using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TandC.GeometryAstro.Utilities
{
    public class ObjectPool<T>
    {
        private readonly Func<T> _preloadFunc;
        private readonly Action<T> _getAction;
        private readonly Action<T> _returnAction;

        private Queue<T> _pool;
        private List<T> _active;

        public ObjectPool(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount)
        {
            _preloadFunc = preloadFunc;
            _getAction = getAction;
            _returnAction = returnAction;

            _pool = new Queue<T>();
            _active = new List<T>();

            if (preloadFunc == null)
            {
                Debug.LogWarning("Preload func is null!");
                return;
            }

            for (int i = 0; i < preloadCount; i++)
            {
                Return(preloadFunc());
            }
        }

        public T Get()
        {
            T item = _pool.Count > 0 ? _pool.Dequeue() : _preloadFunc();
            _getAction(item);
            _active.Add(item);

            return item;
        }

        public void Return(T item)
        {
            if (item == null)
            {
                return;
            }

            _returnAction(item);
            _pool.Enqueue(item);
            _active.Remove(item);
        }

        public void ReturnAll()
        {
            foreach (T item in _active.ToArray())
            {
                Return(item);
            }
        }

        public List<T> GetAllItemInPool() 
        {
            return _pool.ToList();
        }

        public List<T> GetAllActiveItem()
        {
            return _active;
        }

        public void Dispose()
        {
            _pool.Clear();
            _pool = null;

            _active.Clear();
            _active = null;
        }
    }
}