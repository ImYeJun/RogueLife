using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using System.Collections.Generic;

namespace View.BattleView
{
    public class DeckViewSystem : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>, IBackgroundClickDetector
    {
        [SerializeField] private GameObject battleCardView;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private float handWidth = 5f;       
        [SerializeField] private float handHeight = 1f;      
        [SerializeField] private float maxCardAngle = 15f;   

        private List<BattleCardView> cardViews = new List<BattleCardView>();
        private BattleCardView focusedCardView;
        private int focusedCardViewIndex;

        public override void OnInitialized()
        {
            cardViews = new List<BattleCardView>();

            eventBus.Subscribe<CardDrawed>(OnCardDrawed);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<CardDrawed>(OnCardDrawed);
        }

        public void OnCardDrawed(CardDrawed payload)
        {
            cardViews.Add(CreateBattleCardView(payload.Card));
            DrawHandCards();
        }

        private BattleCardView CreateBattleCardView(Card card)
        {
            var instantiatedCard = Instantiate(battleCardView, cardContainer);
            
            var cardView = instantiatedCard.GetComponent<BattleCardView>();
            cardView.Initialize(card, OnCardClicked);

            return cardView;
        }

        private void DrawHandCards()
        {
            for (int i = 0; i < cardViews.Count; i++)
            {
                var view = cardViews[i];
                
                view.transform.SetSiblingIndex(i);

                PositionCard(view, i, cardViews.Count);
            }

            if (focusedCardView != null)
            {
                focusedCardViewIndex = cardViews.IndexOf(focusedCardView);
                focusedCardView.transform.SetAsLastSibling();
            }
        }

        private void PositionCard(BattleCardView cardView, int cardIndex, int totalCount)
        {
            float layoutProgress = (totalCount <= 1) ? 0.5f : (float)cardIndex / (totalCount - 1);

            float normalizedX = (layoutProgress * 2f) - 1f;
            
            float targetX = normalizedX * handWidth;
            float targetY = (-normalizedX * normalizedX + 1f) * handHeight;
            float targetAngle = Mathf.Lerp(maxCardAngle, -maxCardAngle, layoutProgress);
            
            cardView.SetLayoutTransform(new Vector3(targetX, targetY, 0f), new Vector3(0f, 0f, targetAngle));
        }

        public void OnCardClicked(BattleCardView cardView)
        {
            if (focusedCardView == cardView)
            {
                Debug.Log($"{focusedCardView.Card.CurrentName} 실행!, CardType : {focusedCardView.Card.TargetType}");
            }
            else
            {
                if (focusedCardView != null)
                {
                    focusedCardView.Unfocus();
                    focusedCardView.transform.SetSiblingIndex(focusedCardViewIndex);
                }

                focusedCardView = cardView;
                focusedCardViewIndex = cardViews.IndexOf(cardView);
                focusedCardView.transform.SetAsLastSibling();
                
                focusedCardView.Focus();
            }
        }

        public void OnBackgroundClicked()
        {
            if (focusedCardView is null) { return; }

            focusedCardView.Unfocus();
            
            focusedCardView.transform.SetSiblingIndex(focusedCardViewIndex);
            
            focusedCardView = null;
        }
    }
}