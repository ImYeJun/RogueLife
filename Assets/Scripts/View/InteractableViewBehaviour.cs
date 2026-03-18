using ViewEvent.Core;

namespace View.Core
{
    public abstract class InteractableViewBehaviour<TEvent, TCommander> : ViewBehaviour<TEvent> 
        where TEvent : IViewEvent 
        where TCommander : IViewCommander
    {
        protected TCommander commander;

        public void Initialize(System.Random random, ViewEventBus<TEvent> eventBus, PresentationManager presentationManager, TCommander commander)
        {
            this.commander = commander;
            Initialize(random, eventBus, presentationManager);
        }

        public void Initialize(ViewEventBus<TEvent> eventBus, PresentationManager presentationManager, TCommander commander)
        {
            Initialize(new System.Random(), eventBus, presentationManager, commander);
        }
    }
}