using System;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Global;
using View.ScheduleView.Deck;

namespace View.BattleView
{
    public class BattleCardSlotView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject overlap;

        private SharedCardView sharedCardView;
        private Action<BattleCardSlotView> OnSlotClicked;
        public Card CurrentCard => sharedCardView.Card;

        private bool isFocused = false;


        public void Activate(Card card, Action<BattleCardSlotView> onSlotClicked)
        {
            sharedCardView = GetComponent<SharedCardView>();

            OnUnfocus();
            sharedCardView.SetCard(card);
            OnSlotClicked = onSlotClicked;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!isFocused)
            {
                OnSlotClicked.Invoke(this);
            }
        }

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