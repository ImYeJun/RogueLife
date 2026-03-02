using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class DeckViewCloseButton : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private GameObject deckView;

        public override void OnInitialized()
        {
        }

        public override void OnDestroy()
        {
        }

        public void OnPressed()
        {
            deckView.SetActive(false);
        }
    }
}
