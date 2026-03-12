using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using System.Linq;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace View.BattleView
{
    public class DeckViewSystem : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>, IBackgroundClickDetector
    {
        [Header("Behaviour")]
        [SerializeField] private RectTransform handDeckRectransform;
        [SerializeField] private CanvasGroup handDeckCanvasGroup;
        [SerializeField] private GameObject battleCardView;
        [SerializeField] private CardDescriptionView cardDescriptionView;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private float handWidth = 5f;       
        [SerializeField] private float handHeight = 1f;      
        [SerializeField] private float maxCardAngle = 15f;   
        [SerializeField] private CardActivateSystem cardActivateSystem;

        [Header("Open Hand Deck Presentation")]
        [SerializeField] private Vector2 openedHandDeckPosition;
        [SerializeField] private float openHandDeckDuration;
        [SerializeField] private Ease openHandDeckEasingType;

        [Header("Close Hand Deck Presentation")]
        [SerializeField] private Vector2 closedHandDeckPosition;
        [SerializeField] private float closeHandDeckDuration;
        [SerializeField] private Ease closeHandDeckEasingType;

        private bool isHandDeckOpened;
        private Tween currentHandDeckTween;

        private List<BattleCardView> cardViews = new List<BattleCardView>();
        private BattleCardView focusedCardView;
        private int focusedCardViewIndex;

        public override void OnInitialized()
        {
            handDeckRectransform.anchoredPosition = openedHandDeckPosition;
            isHandDeckOpened = true;
            cardDescriptionView.Unfocus();
            cardViews = new List<BattleCardView>();

            eventBus.Subscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus.Subscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
            eventBus.Subscribe<CardDrawed>(OnCardDrawed);
            eventBus.Subscribe<CardDiscarded>(OnCardDiscarded);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus?.Unsubscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
            eventBus?.Unsubscribe<CardDrawed>(OnCardDrawed);
            eventBus?.Unsubscribe<CardDiscarded>(OnCardDiscarded);
        }

        private void OnPlayerTurnStarted(PlayerTurnStarted payload)
        {
            if (isHandDeckOpened) { return ;}

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnStarted_OpenHandDeck, OpenHandDeckPresentation());
            isHandDeckOpened = true;
        }
        private void OnPlayerTurnEnded(PlayerTurnEnded payload)
        {
            if (!isHandDeckOpened) { return; }

            UnfocusFoucsedCard();
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnStarted_OpenHandDeck, CloseHandDeckPresentation());
            isHandDeckOpened = false;
        }

        private IEnumerator OpenHandDeckPresentation()
        {
            currentHandDeckTween?.Kill();

            handDeckCanvasGroup.blocksRaycasts = false;

            currentHandDeckTween = handDeckRectransform.DOAnchorPos(openedHandDeckPosition, openHandDeckDuration).SetEase(openHandDeckEasingType);
            yield return currentHandDeckTween.WaitForCompletion();

            handDeckRectransform.anchoredPosition = openedHandDeckPosition;
            handDeckCanvasGroup.blocksRaycasts = true;
        }
        private IEnumerator CloseHandDeckPresentation()
        {
            currentHandDeckTween?.Kill();

            handDeckCanvasGroup.blocksRaycasts = false;

            currentHandDeckTween = handDeckRectransform.DOAnchorPos(closedHandDeckPosition, closeHandDeckDuration).SetEase(closeHandDeckEasingType);
            yield return currentHandDeckTween.WaitForCompletion();

            handDeckRectransform.anchoredPosition = closedHandDeckPosition;
            handDeckCanvasGroup.blocksRaycasts = true;
        }

        public void OnCardDrawed(CardDrawed payload)
        {
            cardViews.Add(CreateBattleCardView(payload.Card));
            DrawHandCards();
        }

        public void OnCardDiscarded(CardDiscarded payload)
        {
            var view = cardViews.FirstOrDefault(view => view.Card == payload.Card);
            
            if (view is null)
            {
                throw new InvalidOperationException($"[DeckViewSystem] Given UI isn't presenting {payload.Card}");
            }

            cardViews.Remove(view);
            if (view == focusedCardView)
            {
                focusedCardView = null;
                cardDescriptionView.Unfocus();
            }
            Destroy(view.gameObject);
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
            
            float currentWidth = Mathf.Min(handWidth, handWidth * (totalCount / 5f));
            float targetX = normalizedX * currentWidth;
            float targetY = (-normalizedX * normalizedX + 1f) * handHeight;
            float targetAngle = Mathf.Lerp(maxCardAngle, -maxCardAngle, layoutProgress);
            
            cardView.SetLayoutTransform(new Vector3(targetX, targetY, 0f), new Vector3(0f, 0f, targetAngle));
        }

        public void OnCardClicked(BattleCardView cardView)
        {
            if (focusedCardView == cardView)
            {
                cardActivateSystem.UseCard(cardView.Card);
            }
            else
            {
                if (focusedCardView != null)
                {
                    UnfocusFoucsedCard();
                }

                focusedCardView = cardView;
                focusedCardViewIndex = cardViews.IndexOf(cardView);
                focusedCardView.transform.SetAsLastSibling();
                
                focusedCardView.Focus();
                cardDescriptionView.Focus(focusedCardView);
            }
        }

        public void OnBackgroundClicked()
        {
            if (focusedCardView is null) { return; }

            UnfocusFoucsedCard();
        }

        private void UnfocusFoucsedCard()
        {   
            if (focusedCardView is null) { return; }

            focusedCardView.Unfocus();
            cardDescriptionView.Unfocus();
            focusedCardView.transform.SetSiblingIndex(focusedCardViewIndex);
            focusedCardView = null;
        }

#if UNITY_EDITOR
        [ContextMenu("Test Open Hand Deck")]
        public void TestOpenHandDeck()
        {
            handDeckRectransform.anchoredPosition = closedHandDeckPosition;
            StartCoroutine(OpenHandDeckPresentation());
            isHandDeckOpened = true; 
        }

        [ContextMenu("Test Close Hand Deck")]
        public void TestCloseHandDeck()
        {
            handDeckRectransform.anchoredPosition = openedHandDeckPosition;
            StartCoroutine(CloseHandDeckPresentation());
            isHandDeckOpened = false;
        }
#endif
    }
}