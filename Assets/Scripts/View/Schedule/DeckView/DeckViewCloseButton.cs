using UnityEngine;
using UnityEngine.Events;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Deck
{
    public class DeckViewCloseButton : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private GameObject deckView;
        [SerializeField] private UnityEvent<bool> SetCardInspectorActive;

        public override void OnInitialized()
        {
        }

        public override void OnDestroy()
        {
        }

        public void OnPressed()
        {
            deckView.SetActive(false);
            SetCardInspectorActive.Invoke(false);
        }
    }
}
