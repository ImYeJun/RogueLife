using UnityEngine;
using ViewEvent.Core;

namespace View.Core
{
    public abstract class ViewBehaviour<TEvent> : MonoBehaviour where TEvent : IViewEvent
    {
        protected ViewEventBus<TEvent> eventBus;
        protected PresentationManager presentationManager;

        public void Initialize(ViewEventBus<TEvent> eventBus, PresentationManager presentationManager)
        {
            this.eventBus = eventBus;
            this.presentationManager = presentationManager;

            OnInitialized();
        }

        public abstract void OnInitialized();
        public abstract void OnDestroy();
    }
} 