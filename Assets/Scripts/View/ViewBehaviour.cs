using UnityEngine;
using ViewEvent;
using ViewEvent.Core;

namespace View.Core
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