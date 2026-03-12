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

        [Header("Draw Card Presentation")]
        [SerializeField] private RectTransform drawDeckPosition;

        [Header("Dicard Card Presentation")]
        [SerializeField] private RectTransform graveDeckPosition;
        [SerializeField] private RectTransform discardControlPointOffset;

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

        private void OnCardDrawed(CardDrawed payload)
        {
            var newCardView = CreateBattleCardView(payload.Card);
            newCardView.gameObject.SetActive(false);
            cardViews.Add(newCardView);

            presentationManager.Enqueue(payload.SequenceId, -1, DrawCardPresentation(newCardView, new List<BattleCardView>(cardViews)));
        }

        private IEnumerator DrawCardPresentation(BattleCardView drawedCardView, List<BattleCardView> currentCardViews)
        {
            RectTransform drawCardRect = drawedCardView.rectTransform;

            drawCardRect.position = drawDeckPosition.position;
            drawCardRect.rotation = drawDeckPosition.rotation; 

            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < currentCardViews.Count; i++)
            {
                var view = currentCardViews[i];
                view.transform.SetSiblingIndex(i);
                view.gameObject.SetActive(true);

                GetCardPositionAngle(i, currentCardViews.Count, out Vector3 targetPos, out Vector3 targetAngle);
                view.SetBaseLayoutTransform(targetPos, targetAngle);

                sequence.Join(view.rectTransform.DOAnchorPos(targetPos, 0.1f).SetEase(Ease.OutQuad));
                sequence.Join(view.transform.DORotate(targetAngle, 0.1f, RotateMode.Fast).SetEase(Ease.OutQuad));
            }

            yield return sequence.WaitForCompletion();
        }

        private void OnCardDiscarded(CardDiscarded payload)
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
            
            presentationManager.Enqueue(payload.SequenceId, 0, DiscardCardPresentation(view, new List<BattleCardView>(cardViews)));
        }
        private IEnumerator DiscardCardPresentation(BattleCardView discardCard, List<BattleCardView> currentCardViews)
        {
            var discardCardRect = discardCard.rectTransform;

            Sequence sequence = DOTween.Sequence();

            Vector3 startPos = discardCardRect.position;
            Vector3 endPos = graveDeckPosition.position;
            Vector3 controlPos = discardControlPointOffset.position;
            float t = 0f;
            sequence.Join(DOTween.To(() => t, x => 
            {
                t = x; 
                discardCardRect.position = CalculateQuadraticBezierPoint(t, startPos, controlPos, endPos);
            }, 1f, 1f).SetEase(Ease.InOutCubic));

            // sequence.Join(discardCardRect.DOMove(graveDeckPosition.position, 1).SetEase(Ease.OutQuad));
            sequence.Join(discardCardRect.DORotate(graveDeckPosition.rotation.eulerAngles, 1).SetEase(Ease.OutQuad));
            sequence.Join(discardCardRect.DOScale(0.4f, 1)).SetEase(Ease.OutQuad);

            for (int i = 0; i < currentCardViews.Count; i++)
            {
                var view = currentCardViews[i];
                view.transform.SetSiblingIndex(i);
                view.gameObject.SetActive(true);

                GetCardPositionAngle(i, currentCardViews.Count, out Vector3 targetPos, out Vector3 targetAngle);
                view.SetBaseLayoutTransform(targetPos, targetAngle);

                sequence.Join(view.rectTransform.DOAnchorPos(targetPos, 0.3f).SetEase(Ease.OutQuad));
                sequence.Join(view.transform.DORotate(targetAngle, 0.3f, RotateMode.Fast).SetEase(Ease.OutQuad));
            }

            yield return sequence.WaitForCompletion();

            Destroy(discardCard.gameObject);
        }
        private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            
            Vector3 p = uu * p0; 
            p += 2 * u * t * p1; 
            p += tt * p2;        
            
            return p;
        }

        private BattleCardView CreateBattleCardView(Card card)
        {
            var instantiatedCard = Instantiate(battleCardView, cardContainer);
            
            var cardView = instantiatedCard.GetComponent<BattleCardView>();
            cardView.Initialize(card, OnCardClicked);

            return cardView;
        }

        // private void DrawHandCards()
        // {
        //     for (int i = 0; i < cardViews.Count; i++)
        //     {
        //         var view = cardViews[i];
                
        //         view.transform.SetSiblingIndex(i);

        //         GetCardPositionAngle(i, cardViews.Count, out Vector3 position, out Vector3 angle);
        //         view.SetLayoutTransform(position, angle);
        //     }

        //     if (focusedCardView != null)
        //     {
        //         focusedCardViewIndex = cardViews.IndexOf(focusedCardView);
        //         focusedCardView.transform.SetAsLastSibling();
        //     }
        // }

        private void GetCardPositionAngle(int cardIndex, int totalCount, out Vector3 position, out Vector3 angle)
        {
            float layoutProgress = (totalCount <= 1) ? 0.5f : (float)cardIndex / (totalCount - 1);

            float normalizedX = (layoutProgress * 2f) - 1f;
            
            float currentWidth = Mathf.Min(handWidth, handWidth * (totalCount / 5f));
            float targetX = normalizedX * currentWidth;
            float targetY = (-normalizedX * normalizedX + 1f) * handHeight;
            float targetAngle = Mathf.Lerp(maxCardAngle, -maxCardAngle, layoutProgress);
            

            position = new Vector3(targetX, targetY, 0f);
            angle = new Vector3(0f, 0f, targetAngle);
        }

        private IEnumerator CardDiscardPresentation()
        {
            yield return null;
        }
        private IEnumerator CardRevivePresentation()
        {
            yield return null;
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
        [ContextMenu("Test Open Hand Deck (Direct)")]
        public void TestOpenHandDeck()
        {
            StartCoroutine(DelayedTestRoutine(OpenHandDeckPresentation()));
            isHandDeckOpened = true;
        }

        [ContextMenu("Test Close Hand Deck (Direct)")]
        public void TestCloseHandDeck()
        {
            if (!Application.isPlaying) return;
            StartCoroutine(DelayedTestRoutine(CloseHandDeckPresentation()));
            isHandDeckOpened = false;
        }

        private IEnumerator DelayedTestRoutine(IEnumerator targetPresentation)
        {
            yield return new WaitForSeconds(0.5f); 
            yield return StartCoroutine(targetPresentation);
        }
#endif
    }
}