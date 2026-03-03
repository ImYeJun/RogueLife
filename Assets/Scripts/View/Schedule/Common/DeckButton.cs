using UnityEngine;
using View.Core;
using View.ScheduleView.Deck;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class DeckButton : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private DeckInventoryView deckView;

        public override void OnInitialized()
        {
        }

        public override void OnDestroy()
        {
        }

        public void OnPressed()
        {
            deckView.OnViewOpened();
        }
    }
}
