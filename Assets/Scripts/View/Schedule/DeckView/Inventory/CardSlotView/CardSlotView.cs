using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Deck
{
    public abstract class CardSlotView : ViewBehaviour<IScheduleViewEvent>, IPointerClickHandler
    {
        [SerializeField] private GameObject overlap;
        protected IScheduleViewCommander commander;

        private SharedCardView sharedCardView;
        private Action<CardSlotView> OnSlotClicked;
        public Card CurrentCard => sharedCardView.Card;

        private bool isFocused = false;

        private void Awake() {
            sharedCardView = GetComponent<SharedCardView>();
        }

        public override void OnInitialized()
        {
        }
        public override void OnDestroy()
        {
        }

        public void Activate(Card card, Action<CardSlotView> onSlotClicked, IScheduleViewCommander commander)
        {
            OnUnfocus();
            sharedCardView.SetCard(card);
            OnSlotClicked = onSlotClicked;
            this.commander = commander;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isFocused)
            {
                OnFocusedClicked();
            }
            else
            {
                OnSlotClicked.Invoke(this);
            }
        }
        protected abstract void OnFocusedClicked();

        public void OnFocused()
        {
            isFocused = true;
            overlap.SetActive(true);
        }
        public void OnUnfocus()
        {
            isFocused = false;
            overlap.SetActive(false);
        }
    }
}
