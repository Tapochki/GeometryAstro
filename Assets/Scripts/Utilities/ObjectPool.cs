using System;
using System.Collections.Generic;

namespace TandC.Utilities
{
    public class ObjectPool<T>
    {
        /*
         * Simple guid how to use
         *
         * First need to create Preload func like => public Missle Preload() => Instantiane(prefab);
         *
         * Where Missle is our class with object that need to spawn.
         *
         * Second need to create actions where [get] is => public void GetAction(Missle missle) => missle.gameObject.SetActive(true);
         *
         * and [return] is => public void ReturnAction(Missle missle) => missle.gameObject.SetActive(false);
         *
         * After this simple manipulations we have pool of missles. When pool haven't empty object (mean all objects is active) would created one new object.
         *
         * preloadCount need for creating starting object that in future would panipulated by script.
         *
         * init of pool recomended doing in Awake() func with exited params => _misslesPool = new ObjectPool<Missles>(Preload, GetAction, ReturnAction, 20);
         *
         * where _misslesPool is => private ObjectPool<Missles> _misslesPool;
         *
         * Good luck!
         */

        private readonly Func<T> _preloadFunc;
        private readonly Action<T> _getAction;
        private readonly Action<T, bool> _returnAction;

        private Queue<T> _pool;
        private List<T> _active;

        public ObjectPool(Func<T> preloadFunc, Action<T> getAction, Action<T, bool> returnAction, int preloadCount)
        {
            _preloadFunc = preloadFunc;
            _getAction = getAction;
            _returnAction = returnAction;

            _pool = new Queue<T>();
            _active = new List<T>();

            if (preloadFunc == null)
            {
                Utilities.Logger.Log("Preload func is null!", Settings.LogTypes.Warning);
                return;
            }

            for (int i = 0; i < preloadCount; i++)
            {
                Return(preloadFunc(), true);
            }
        }

        public T Get()
        {
            T item = _pool.Count > 0 ? _pool.Dequeue() : _preloadFunc();
            _getAction(item);
            _active.Add(item);

            return item;
        }

        public void Return(T item, bool isInitializeReturn)
        {
            if (item == null)
            {
                return;
            }

            _returnAction(item, isInitializeReturn);
            _pool.Enqueue(item);
            _active.Remove(item);
        }

        public void ReturnAll()
        {
            foreach (T item in _active.ToArray())
            {
                Return(item, true);
            }
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