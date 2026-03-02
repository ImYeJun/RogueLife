using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class CardSlotView : ViewBehaviour<IScheduleViewEvent>, IPointerClickHandler
    {
        private SharedCardView sharedCardView;
        private UnityEvent<Card> OnSlotClicked;

        private void Awake() {
            sharedCardView = GetComponent<SharedCardView>();
        }
        public override void OnInitialized()
        {
        }
        public override void OnDestroy()
        {
        }
        public void Activate(Card card, UnityEvent<Card> onSlotClicked)
        {
            sharedCardView.SetCard(card);
            OnSlotClicked = onSlotClicked;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSlotClicked.Invoke(sharedCardView.Card);
        }
    }
}
