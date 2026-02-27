using UnityEngine;
using ViewEvent;

namespace View
{
    public abstract class ViewBehaviour<TEvent> : MonoBehaviour where TEvent : IViewEvent
    {
        protected ViewEventBus<TEvent> eventBus;

        public void Initialize(ViewEventBus<TEvent> eventBus)
        {
            this.eventBus = eventBus;

            OnInitialzied();
        }

        public abstract void OnInitialzied();
    }
} 