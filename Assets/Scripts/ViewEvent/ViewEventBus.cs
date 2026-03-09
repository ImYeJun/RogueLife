using System;
using System.Collections.Generic;

namespace ViewEvent.Core
{
    public abstract class ViewEventBus<TEvent> where TEvent : IViewEvent
    {
        protected Dictionary<Type, List<Delegate>> observerDict = new Dictionary<Type, List<Delegate>>();

        public void Publish<T>(T payload) where T : TEvent
        {
            var type = typeof(T);

            if (!observerDict.ContainsKey(type)) { return; }

            var observers = observerDict[type];
            for (int i = observers.Count - 1; i >= 0; i--)
            {
                var materializedAction = (Action<T>)observers[i];
                materializedAction.Invoke(payload);
            }
        }
        public void Subscribe<T>(Action<T> observer) where T : TEvent
        {
            var type = typeof(T);

            if (!observerDict.ContainsKey(type))
            {
                observerDict[type] = new List<Delegate>();
            }

            var list = observerDict[type];
            if (list.Contains(observer))
            {
                UnityEngine.Debug.LogWarning($"[{GetType()}] The given observer is already subscribing.");
                return;
            }
            
            list.Add(observer);
        }
        public void Unsubscribe<T>(Action<T> observer) where T : TEvent
        {
            var type = typeof(T);

            if (!observerDict.TryGetValue(type, out var list) || !list.Contains(observer))
            {
                UnityEngine.Debug.LogWarning($"[{GetType()}] The given observer is not subscribing.");
                return;
            }

            list.Remove(observer);
        }

        internal void Publish<T>(object onInitialDeckSettled)
        {
            throw new NotImplementedException();
        }
    }
}