using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using System.Linq;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

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
        
        [Header("- Target Card (Drawing)")]
        [SerializeField] private float drawTargetMoveDuration = 0.4f;
        [SerializeField] private Ease drawTargetMoveEase = Ease.OutQuad;
        [SerializeField] private float drawTargetRotateDuration = 0.4f;
        [SerializeField] private Ease drawTargetRotateEase = Ease.OutQuad;
        [SerializeField] private float drawTargetScaleDuration = 0.3f;
        [SerializeField] private Ease drawTargetScaleEase = Ease.OutBack;

        [Header("- Existing Cards (Rearranging)")]
        [SerializeField] private float drawExistingMoveDuration = 0.3f;
        [SerializeField] private Ease drawExistingMoveEase = Ease.OutQuad;
        [SerializeField] private float drawExistingRotateDuration = 0.3f;
        [SerializeField] private Ease drawExistingRotateEase = Ease.OutQuad;

        [Header("Discard Card Presentation")]
        [SerializeField] private RectTransform graveDeckPosition;
        [SerializeField] private RectTransform discardControlPointOffset;
        
        [Header("- Target Card (Discarding)")]
        [SerializeField] private float discardTargetMoveDuration = 0.5f;
        [SerializeField] private Ease discardTargetMoveEase = Ease.InQuad;
        [SerializeField] private float discardTargetRotateDuration = 0.5f;
        [SerializeField] private Ease discardTargetRotateEase = Ease.InQuad;
        [SerializeField] private float discardTargetScaleDuration = 0.5f;
        [SerializeField] private Ease discardTargetScaleEase = Ease.InBack;

        [Header("- Existing Cards (Rearranging)")]
        [SerializeField] private float discardExistingMoveDuration = 0.3f;
        [SerializeField] private Ease discardExistingMoveEase = Ease.OutQuad;
        [SerializeField] private float discardExistingRotateDuration = 0.3f;
        [SerializeField] private Ease discardExistingRotateEase = Ease.OutQuad;

        [Header("Restore Card Presentation")]
        [SerializeField, FormerlySerializedAs("restoreCardDuration")] private float restoreMoveCardDuration;
        [SerializeField, FormerlySerializedAs("restorCardEase")] private Ease restoreMoveCardEase;

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
            eventBus.Subscribe<CardRestored>(OnCardRestored);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus?.Unsubscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
            eventBus?.Unsubscribe<CardDrawed>(OnCardDrawed);
            eventBus?.Unsubscribe<CardDiscarded>(OnCardDiscarded);
            eventBus?.Unsubscribe<CardRestored>(OnCardRestored);
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

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardDrawed_HandDeckPresentation, DrawCardPresentation(newCardView, new List<BattleCardView>(cardViews)));
        }
        private IEnumerator DrawCardPresentation(BattleCardView drawedCardView, List<BattleCardView> currentCardViews)
        {
            RectTransform drawCardRect = drawedCardView.rectTransform;

            drawCardRect.position = drawDeckPosition.position;
            drawCardRect.rotation = drawDeckPosition.rotation; 
            drawCardRect.localScale = Vector3.zero; 

            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < currentCardViews.Count; i++)
            {
                var view = currentCardViews[i];
                view.transform.SetSiblingIndex(i);
                view.gameObject.SetActive(true);

                GetCardPositionAngle(i, currentCardViews.Count, out Vector3 targetPos, out Vector3 targetAngle);
                view.SetBaseLayoutTransform(targetPos, targetAngle);

                if (view == drawedCardView)
                {
                    sequence.Join(view.rectTransform.DOAnchorPos(targetPos, drawTargetMoveDuration).SetEase(drawTargetMoveEase));
                    sequence.Join(view.transform.DORotate(targetAngle, drawTargetRotateDuration, RotateMode.Fast).SetEase(drawTargetRotateEase));
                    sequence.Join(view.transform.DOScale(Vector3.one, drawTargetScaleDuration).SetEase(drawTargetScaleEase));
                }
                else
                {
                    sequence.Join(view.rectTransform.DOAnchorPos(targetPos, drawExistingMoveDuration).SetEase(drawExistingMoveEase));
                    sequence.Join(view.transform.DORotate(targetAngle, drawExistingRotateDuration, RotateMode.Fast).SetEase(drawExistingRotateEase));
                }
            }

            yield return sequence.WaitForCompletion();
        }

        private void OnCardDiscarded(CardDiscarded payload)
        {
            var view = cardViews.FirstOrDefault(v => v.Card == payload.Card);
            
            if (view is null)
            {
                throw new InvalidOperationException($"[DeckViewSystem/OnCardDiscarded] Given UI isn't presenting card ID: {payload.Card}");
            }

            cardViews.Remove(view);
            if (view == focusedCardView)
            {
                focusedCardView = null;
                cardDescriptionView.Unfocus();
            }
            
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardDiscarded_HandDeckPresentation, DiscardCardPresentation(view, new List<BattleCardView>(cardViews), payload.Destination));
        }
        private IEnumerator DiscardCardPresentation(BattleCardView discardCard, List<BattleCardView> currentCardViews, BattleDeckType destination)
        {
            var discardCardRect = discardCard.rectTransform;

            Sequence sequence = DOTween.Sequence();

            Vector3 startPos = discardCardRect.position;
            Vector3 endPos = destination switch 
            {
                BattleDeckType.DRAW => drawDeckPosition.position,
                BattleDeckType.GRAVE => graveDeckPosition.position,
                _ => throw new InvalidOperationException($"[DeckViewSystem] {destination} is not valid.")
            };
            Vector3 controlPos = discardControlPointOffset.position;
            
            float t = 0f;
            
            sequence.Join(DOTween.To(() => t, x => 
            {
                t = x; 
                discardCardRect.position = CalculateQuadraticBezierPoint(t, startPos, controlPos, endPos);
            }, 1f, discardTargetMoveDuration).SetEase(discardTargetMoveEase));

            sequence.Join(discardCardRect.DORotate(graveDeckPosition.rotation.eulerAngles, discardTargetRotateDuration).SetEase(discardTargetRotateEase));
            sequence.Join(discardCardRect.DOScale(0.2f, discardTargetScaleDuration).SetEase(discardTargetScaleEase));

            for (int i = 0; i < currentCardViews.Count; i++)
            {
                var view = currentCardViews[i];
                view.transform.SetSiblingIndex(i);
                view.gameObject.SetActive(true);

                GetCardPositionAngle(i, currentCardViews.Count, out Vector3 targetPos, out Vector3 targetAngle);
                view.SetBaseLayoutTransform(targetPos, targetAngle);

                sequence.Join(view.rectTransform.DOAnchorPos(targetPos, discardExistingMoveDuration).SetEase(discardExistingMoveEase));
                sequence.Join(view.transform.DORotate(targetAngle, discardExistingRotateDuration, RotateMode.Fast).SetEase(discardExistingRotateEase));
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

        private void OnCardRestored(CardRestored payload)
        {
            var view = CreateBattleCardView(payload.Card);
            view.gameObject.SetActive(false);
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardRestored_HandDeckPresentation, RestoreCardPresentation(view));
        }
        private IEnumerator RestoreCardPresentation(BattleCardView cardView)
        {
            cardView.gameObject.SetActive(true);
            cardView.transform.position = graveDeckPosition.position;

            var sequence = DOTween.Sequence();
            sequence.Join(cardView.transform.DOMove(drawDeckPosition.position, restoreMoveCardDuration).SetEase(restoreMoveCardEase));

            yield return sequence.WaitForCompletion();
            Destroy(cardView.gameObject);
        }


        private BattleCardView CreateBattleCardView(Card card)
        {
            var instantiatedCard = Instantiate(battleCardView, cardContainer);
            
            var cardView = instantiatedCard.GetComponent<BattleCardView>();
            cardView.Initialize(card, OnCardClicked);

            return cardView;
        }

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
            if (!Application.isPlaying) return;
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
            yield return new WaitForSeconds(0.4f); 
            yield return StartCoroutine(targetPresentation);
        }
#endif
    }
}