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
        [SerializeField] private Transform processingCardContainer;
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
        private BattleCardView processingCardView;

        public override void OnInitialized()
        {
            handDeckRectransform.anchoredPosition = openedHandDeckPosition;
            isHandDeckOpened = true;
            cardDescriptionView.Unfocus();
            cardViews = new List<BattleCardView>();

            cardActivateSystem.OnCardProcessingPrepared = OnCardProcessed;
            cardActivateSystem.SetHandCardInteractable = SetHandCardInteractable;
            cardActivateSystem.IsProcessingCard = IsProcessingCard;

            eventBus.Subscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus.Subscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
            eventBus.Subscribe<CardDrawed>(OnCardDrawed);
            eventBus.Subscribe<CardDiscarded>(OnCardDiscarded);
            eventBus.Subscribe<CardRestored>(OnCardRestored);
            eventBus.Subscribe<CardTriggerResolved>(OnCardTriggerResolved);
            eventBus.Subscribe<CardActivationCancelled>(OnCardActivationCancelled); 
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus?.Unsubscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
            eventBus?.Unsubscribe<CardDrawed>(OnCardDrawed);
            eventBus?.Unsubscribe<CardDiscarded>(OnCardDiscarded);
            eventBus?.Unsubscribe<CardRestored>(OnCardRestored);
            eventBus?.Unsubscribe<CardTriggerResolved>(OnCardTriggerResolved);
            eventBus?.Unsubscribe<CardActivationCancelled>(OnCardActivationCancelled);
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

            SetHandCardInteractable(false);

            currentHandDeckTween = handDeckRectransform.DOAnchorPos(openedHandDeckPosition, openHandDeckDuration).SetEase(openHandDeckEasingType);
            yield return currentHandDeckTween.WaitForCompletion();

            handDeckRectransform.anchoredPosition = openedHandDeckPosition;
            SetHandCardInteractable(true);
        }

        private IEnumerator CloseHandDeckPresentation()
        {
            currentHandDeckTween?.Kill();

            SetHandCardInteractable(false);

            currentHandDeckTween = handDeckRectransform.DOAnchorPos(closedHandDeckPosition, closeHandDeckDuration).SetEase(closeHandDeckEasingType);
            yield return currentHandDeckTween.WaitForCompletion();

            handDeckRectransform.anchoredPosition = closedHandDeckPosition;
            SetHandCardInteractable(true);
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
            drawedCardView.SetWorldTransform(drawDeckPosition.position, drawDeckPosition.rotation, Vector3.zero);

            Sequence sequence = DOTween.Sequence();
            
            sequence.Join(PlayCardSortPresentation(
                currentCardViews, 
                drawExistingMoveDuration, drawExistingRotateDuration, 
                drawExistingMoveEase, drawExistingRotateEase, 
                excludeTweenView: drawedCardView));

            GetCardPositionAngle(currentCardViews.IndexOf(drawedCardView), currentCardViews.Count, out Vector3 targetPos, out Vector3 targetAngle);
            drawedCardView.SetBaseLayoutTransform(targetPos, targetAngle);

            sequence.Join(drawedCardView.PlayDrawPresentation(
                drawTargetMoveDuration, drawTargetRotateDuration, drawTargetScaleDuration, 
                drawTargetMoveEase, drawTargetRotateEase, drawTargetScaleEase));

            yield return sequence.WaitForCompletion();
        }

        private void OnCardActivationCancelled(CardActivationCancelled payload)
        {
            // 💡 단일 변수 복구
            if (processingCardView == null || processingCardView.Card != payload.Card) return;

            var view = processingCardView;
            processingCardView = null;

            cardViews.Add(view);
            view.transform.SetParent(cardContainer);
            
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardDiscarded_HandDeckPresentation, PlayCancelActivationPresentation());
        }

        private IEnumerator PlayCancelActivationPresentation()
        {
            var sequence = DOTween.Sequence();
            sequence.Join(PlayCardSortPresentation(cardViews, 0.4f, 0.4f, Ease.OutQuad, Ease.OutQuad));
            
            yield return sequence.WaitForCompletion();
            
            if (processingCardView == null)
            {
                SetHandCardInteractable(true);
            }
        }

        private void OnCardDiscarded(CardDiscarded payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardDiscarded_HandDeckPresentation, DiscardCardPresentation(payload.Card, payload.Destination));
        }

        private IEnumerator DiscardCardPresentation(Card discardCardData, BattleDeckType destination)
        {
            var view = cardViews.FirstOrDefault(v => v.Card == discardCardData);
            
            if (view == null && processingCardView != null && processingCardView.Card == discardCardData)
            {
                view = processingCardView;
            }

            if (view is null)
            {
                Debug.Log($"[DeckViewSystem/DiscardCardPresentation] Given UI isn't presenting card ID: {discardCardData.CurrentName}. Skipping discard animation.");
                yield break;
            }

            if (view == processingCardView)
            {
                processingCardView = null;
                SetHandCardInteractable(true);
            }
            else
            {
                cardViews.Remove(view);
                
                if (view == focusedCardView)
                {
                    focusedCardView = null;
                    cardDescriptionView.Unfocus();
                }
            }

            Sequence sequence = DOTween.Sequence();

            Vector3 endPos = destination switch
            {
                BattleDeckType.DRAW => drawDeckPosition.position,
                BattleDeckType.GRAVE => graveDeckPosition.position,
                _ => throw new InvalidOperationException($"[DeckViewSystem/DiscardCardPresentation] {destination} is not valid.")
            };
            Vector3 controlPos = discardControlPointOffset.position;
            Vector3 endRot = graveDeckPosition.rotation.eulerAngles;

            sequence.Join(view.PlayDiscardPresentation(
                endPos, controlPos, endRot,
                discardTargetMoveDuration, discardTargetRotateDuration, discardTargetScaleDuration,
                discardTargetMoveEase, discardTargetRotateEase, discardTargetScaleEase));

            sequence.Join(PlayCardSortPresentation(
                cardViews, 
                discardExistingMoveDuration, discardExistingRotateDuration,
                discardExistingMoveEase, discardExistingRotateEase));

            yield return sequence.WaitForCompletion();

            Destroy(view.gameObject);
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
            
            cardView.SetWorldTransform(graveDeckPosition.position, graveDeckPosition.rotation, Vector3.one);

            var sequence = DOTween.Sequence();
            
            sequence.Join(cardView.PlayRestorePresentation(drawDeckPosition.position, restoreMoveCardDuration, restoreMoveCardEase));

            yield return sequence.WaitForCompletion();
            Destroy(cardView.gameObject);
        }

        private void OnCardTriggerResolved(CardTriggerResolved payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardTriggerResolved_ExtinguishCardView, PlayResolveCardPresentation(payload.Card));
        }

        private IEnumerator PlayResolveCardPresentation(Card cardData)
        {
            if (processingCardView == null || cardData != processingCardView.Card)
            {
                Debug.LogWarning($"[{GetType()}/PlayResolveCardPresentation] Processing card view not found. Skipped.");
                yield break;
            }

            var view = processingCardView;
            processingCardView = null;
            SetHandCardInteractable(true);

            yield return view.PlayFadePresentation(0.5f, Ease.Linear, isFadeIn : true).WaitForCompletion();
            Destroy(view.gameObject);
        }

        private Tween PlayCardSortPresentation(List<BattleCardView> currentCardViews, float moveDuration, float rotateDuration, Ease moveEase, Ease rotateEase, BattleCardView excludeTweenView = null)
        {
            var sequence = DOTween.Sequence();

            for (int i = 0; i < currentCardViews.Count; i++)
            {
                var view = currentCardViews[i];
                view.transform.SetSiblingIndex(i);
                view.gameObject.SetActive(true);

                GetCardPositionAngle(i, currentCardViews.Count, out Vector3 targetPos, out Vector3 targetAngle);
                view.SetBaseLayoutTransform(targetPos, targetAngle);

                if (view != excludeTweenView)
                {
                    sequence.Join(view.PlayMoveToLayoutTransform(moveDuration, rotateDuration, moveEase, rotateEase));
                }
            }

            return sequence;
        }

        private BattleCardView CreateBattleCardView(Card card)
        {
            return CreateBattleCardView(card, cardContainer);
        }
        private BattleCardView CreateBattleCardView(Card card, Transform parent)
        {
            var instantiatedCard = Instantiate(battleCardView, parent);
            
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

        private IEnumerator OnCardProcessed(Card card, bool isTriggering)
        {
            yield return null;

            if (processingCardView is not null)
            {
                throw new InvalidOperationException($"[{GetType()}/OnCardProcessed] Try to process card but it's already processing.");
            }

            UnfocusFoucsedCard();
            var sequence = DOTween.Sequence();

            if (isTriggering)
            {
                processingCardView = CreateBattleCardView(card, processingCardContainer);
                processingCardView.SetLayoutTransform(new Vector3(0, 100, 0), Vector3.zero);

                sequence.Append(processingCardView.PlayFadePresentation(0.5f, Ease.Linear, isFadeIn: false));
            }
            else
            {
                var cardView = cardViews.FirstOrDefault(view => view.Card == card);
                if (cardView is null)
                {
                    throw new InvalidOperationException($"[{GetType()}/OnCardProcessed] Try to process using card but the given card is not presenting.");
                }
                processingCardView = cardView;

                cardViews.Remove(processingCardView);
                processingCardView.transform.SetParent(processingCardContainer);

                processingCardView.SetBaseLayoutTransform(new Vector3(0, 100, 0), Vector3.zero);
                sequence.Append(processingCardView.PlayMoveToLayoutTransform(0.5f, 0.5f, Ease.Linear, Ease.Linear));

                sequence.Join(PlayCardSortPresentation(cardViews, 0.5f, 0.5f, Ease.Linear, Ease.Linear));
            }

            yield return sequence.WaitForCompletion();
        }

        private void SetHandCardInteractable(bool value)
        {
            handDeckCanvasGroup.blocksRaycasts = value;
            handDeckCanvasGroup.interactable = value;
        }

        private bool IsProcessingCard()
        {
            return processingCardView is not null;
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