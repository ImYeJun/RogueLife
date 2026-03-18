using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.ScheduleView.CollectionUpdateView
{
    public class CardView : ItemView {
        private SharedCardView cardView;
        private bool isReflectionText;

        public bool IsReflectionText => isReflectionText; 

        private void Awake() {
            cardView = GetComponent<SharedCardView>();
        }   

        public void Draw(Card card)
        {
            isReflectionText = false;
            cardView.SetCard(card);
            cardView.DrawDescription(isReflectionText);

            PopUp();
        }

        public void DrawDescription(bool isReflection)
        {
            isReflectionText = isReflection;
            cardView.DrawDescription(isReflectionText);
        }
    }
}