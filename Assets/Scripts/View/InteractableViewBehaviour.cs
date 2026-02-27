using ViewEvent;

namespace View
{
    public abstract class InteractableViewBehaviour<TEvent, TCommander> : ViewBehaviour<TEvent> 
        where TEvent : IViewEvent 
        where TCommander : IViewCommander
    {
        protected TCommander commander;

        public void Initialize(ViewEventBus<TEvent> eventBus, TCommander commander)
        {
            Initialize(eventBus);

            this.commander = commander;
        }
    }
}