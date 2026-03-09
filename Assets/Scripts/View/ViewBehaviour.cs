using UnityEngine;
using ViewEvent.Core;

namespace View.Core
{
    public abstract class ViewBehaviour<TEvent> : MonoBehaviour where TEvent : IViewEvent
    {
        protected System.Random random;
        protected ViewEventBus<TEvent> eventBus;
        protected PresentationManager presentationManager;

        public void Initialize(System.Random random, ViewEventBus<TEvent> eventBus, PresentationManager presentationManager)
        {
            this.random = random;
            this.eventBus = eventBus;
            this.presentationManager = presentationManager;

            OnInitialized();
        }

        public void Initialize(ViewEventBus<TEvent> eventBus, PresentationManager presentationManager)
        {
            Initialize(new System.Random(), eventBus, presentationManager);
        }

        public abstract void OnInitialized();
        public abstract void OnDestroy();
    }
} 