using ViewEvent.Core;

namespace View.Core
{
    public abstract class InteractableViewBehaviour<TEvent, TCommander> : ViewBehaviour<TEvent> 
        where TEvent : IViewEvent 
        where TCommander : IViewCommander
    {
        protected TCommander commander;

        public void Initialize(ViewEventBus<TEvent> eventBus, PresentationManager presentationManager, TCommander commander)
        {
            Initialize(eventBus, presentationManager);

            this.commander = commander;
        }
    }
}