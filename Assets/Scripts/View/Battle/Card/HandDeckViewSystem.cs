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
using TMPro;

namespace View.BattleView
{
    public class HandDeckViewSystem : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>, IBackgroundClickDetector
    {
        [Header("Behaviour")]
        [SerializeField] private RectTransform handDeckRectransform;
        [SerializeField] private TextMeshProUGUI handDeckCountText;
        [SerializeField] private CanvasGroup handDeckCanvasGroup;
        [SerializeField] private GameObject battleCardView;
        [SerializeField] private CardDescriptionView cardDescriptionView;
        [SerializeField] private RectTransform cardContainer;
        [SerializeField] private RectTransform processingCardContainer;
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
        [SerializeField] private float drawTargetMoveDuration = 0.4f;
        [SerializeField] private Ease drawTargetMoveEase = Ease.OutQuad;
        [SerializeField] private float drawTargetRotateDuration = 0.4f;
        [SerializeField] private Ease drawTargetRotateEase = Ease.OutQuad;
        [SerializeField] private float drawTargetScaleDuration = 0.3f;
        [SerializeField] private Ease drawTargetScaleEase = Ease.OutBack;
        [SerializeField] private float drawExistingMoveDuration = 0.3f;
        [SerializeField] private Ease drawExistingMoveEase = Ease.OutQuad;
        [SerializeField] private float drawExistingRotateDuration = 0.3f;
        [SerializeField] private Ease drawExistingRotateEase = Ease.OutQuad;

        [Header("Discard Card Presentation")]
        [SerializeField] private RectTransform graveDeckPosition;
        [SerializeField] private RectTransform discardControlPointOffset;
        [SerializeField] private float discardTargetMoveDuration = 0.5f;
        [SerializeField] private Ease discardTargetMoveEase = Ease.InQuad;
        [SerializeField] private float discardTargetRotateDuration = 0.5f;
        [SerializeField] private Ease discardTargetRotateEase = Ease.InQuad;
        [SerializeField] private float discardTargetScaleDuration = 0.5f;
        [SerializeField] private Ease discardTargetScaleEase = Ease.InBack;
        [SerializeField] private float discardExistingMoveDuration = 0.3f;
        [SerializeField] private Ease discardExistingMoveEase = Ease.OutQuad;
        [SerializeField] private float discardExistingRotateDuration = 0.3f;
        [SerializeField] private Ease discardExistingRotateEase = Ease.OutQuad;

        [Header("Restore Card Presentation")]
        [SerializeField, FormerlySerializedAs("restoreCardDuration")] private float restoreMoveCardDuration;
        [SerializeField, FormerlySerializedAs("restorCardEase")] private Ease restoreMoveCardEase;

        [Header("Cancel Activation Presentation")]
        [SerializeField] private float cancelSortMoveDuration;
        [SerializeField] private float cancelSortRotateDuration;
        [SerializeField] private Ease cancelSortMoveEase;
        [SerializeField] private Ease cancelSortRotateEase;

        [Header("Process Card Presentation")]
        [SerializeField] private Vector3 processingCardPosition;
        [SerializeField] private Vector3 processingCardScale = new Vector3(1.2f, 1.2f, 1.2f);
        
        [SerializeField] private float processingCardContainerOffsetY = -150f;
        [SerializeField] private float cardContainerMoveDownDuration = 0.4f;
        [SerializeField] private float cardContainerMoveUpDuration = 0.4f;
        [SerializeField] private Ease cardContainerMoveDownEase = Ease.OutQuad;
        [SerializeField] private Ease cardContainerMoveUpEase = Ease.OutBack;

        [SerializeField] private float processSortMoveDuration;
        [SerializeField] private float processSortRotateDuration;
        [SerializeField] private Ease processSortMoveEase;
        [SerializeField] private Ease processSortRotateEase;

        private bool isHandDeckOpened;
        private Tween currentHandDeckTween;

        private List<BattleCardView> cardViews = new List<BattleCardView>();
        private BattleCardView focusedCardView;
        private int focusedCardViewIndex;

        private BattleCardView processingCardView;
        private float originalCardContainerY;

        public override void OnInitialized()
        {
            handDeckRectransform.anchoredPosition = openedHandDeckPosition;
            isHandDeckOpened = true;
            cardDescriptionView.Unfocus();
            cardViews = new List<BattleCardView>();
            originalCardContainerY = cardContainer.anchoredPosition.y;
            handDeckCountText.gameObject.SetActive(false);

            cardDescriptionView.Initialize(commander.GetStatusEffectData);

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
            eventBus.Subscribe<BattleEnded>(OnBattleEnded);
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
            eventBus?.Unsubscribe<BattleEnded>(OnBattleEnded);
        }

        private void OnPlayerTurnStarted(PlayerTurnStarted payload)
        {
            if (isHandDeckOpened) { 
                handDeckCountText.gameObject.SetActive(true);
                return ;
            }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnStarted_OpenHandDeck, OpenHandDeckPresentation());
            isHandDeckOpened = true;
        }

        private void OnPlayerTurnEnded(PlayerTurnEnded payload)
        {
            if (!isHandDeckOpened) { 
                handDeckCountText.gameObject.SetActive(false);
                return;
            }

            UnfocusFoucsedCard();
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnEnded_CloseHandDeck, CloseHandDeckPresentation());
            isHandDeckOpened = false;
        }

        private void OnBattleEnded(BattleEnded payload)
        {
            handDeckCanvasGroup.interactable = false;
            handDeckCanvasGroup.blocksRaycasts = false;
        }


        private IEnumerator OpenHandDeckPresentation()
        {
            currentHandDeckTween?.Kill();

            SetHandCardInteractable(false);

            handDeckCountText.gameObject.SetActive(true);
            currentHandDeckTween = handDeckRectransform.DOAnchorPos(openedHandDeckPosition, openHandDeckDuration).SetEase(openHandDeckEasingType);
            yield return currentHandDeckTween.WaitForCompletion();

            handDeckRectransform.anchoredPosition = openedHandDeckPosition;
            SetHandCardInteractable(true);
        }

        private IEnumerator CloseHandDeckPresentation()
        {
            currentHandDeckTween?.Kill();

            SetHandCardInteractable(false);

            handDeckCountText.gameObject.SetActive(false);
            currentHandDeckTween = handDeckRectransform.DOAnchorPos(closedHandDeckPosition, closeHandDeckDuration).SetEase(closeHandDeckEasingType);
            yield return currentHandDeckTween.WaitForCompletion();

            handDeckRectransform.anchoredPosition = closedHandDeckPosition;
            SetHandCardInteractable(true);
        }

        private void OnCardDrawed(CardDrawed payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardDrawed_HandDeckPresentation, DrawCardPresentation(payload.Card));
        }

        private IEnumerator DrawCardPresentation(Card cardData)
        {
            var drawedCardView = CreateBattleCardView(cardData);
            drawedCardView.gameObject.SetActive(false);
            cardViews.Add(drawedCardView);

            drawedCardView.SetWorldTransform(drawDeckPosition.position, drawDeckPosition.rotation, Vector3.zero);

            Sequence sequence = DOTween.Sequence();
            
            sequence.Join(PlayCardSortPresentation(
                cardViews, 
                drawExistingMoveDuration, drawExistingRotateDuration, 
                drawExistingMoveEase, drawExistingRotateEase, 
                excludeTweenView: drawedCardView));

            GetCardPositionAngle(cardViews.IndexOf(drawedCardView), cardViews.Count, out Vector3 targetPos, out Vector3 targetAngle);
            drawedCardView.SetBaseLayoutTransform(targetPos, targetAngle, Vector3.one);

            drawedCardView.gameObject.SetActive(true);
            sequence.Join(drawedCardView.PlayDrawPresentation(
                drawTargetMoveDuration, drawTargetRotateDuration, drawTargetScaleDuration, 
                drawTargetMoveEase, drawTargetRotateEase, drawTargetScaleEase));

            DrawHandDeckCountText(cardViews.Count);
            yield return sequence.WaitForCompletion();
        }

        private void OnCardActivationCancelled(CardActivationCancelled payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardDiscarded_HandDeckPresentation, PlayCancelActivationPresentation(payload.Card));
        }

        private IEnumerator PlayCancelActivationPresentation(Card cardData)
        {
            if (processingCardView != null && processingCardView.Card == cardData)
            {
                var view = processingCardView;
                processingCardView = null;
                cardViews.Add(view);
                view.transform.SetParent(cardContainer);
            }

            var sequence = DOTween.Sequence();
            
            sequence.Join(PlayCardSortPresentation(cardViews, cancelSortMoveDuration, cancelSortRotateDuration, cancelSortMoveEase, cancelSortRotateEase));
            sequence.Append(PlayCardContainerMovePresentation(isMovingDown: false));

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

        private void DrawHandDeckCountText(int currentCount)
        {
            handDeckCountText.text = $"{currentCount}/{Constant.BASE_MAX_HAND_ZONE_CARD_COUNT}";
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

            Sequence sequence = DOTween.Sequence();
            if (view == processingCardView)
            {
                sequence.Append(PlayCardContainerMovePresentation(isMovingDown: false));
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

            DrawHandDeckCountText(cardViews.Count);
            yield return sequence.WaitForCompletion();

            Destroy(view.gameObject);
        }

        private void OnCardRestored(CardRestored payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardRestored_HandDeckPresentation, RestoreCardPresentation(payload.Card));
        }

        private IEnumerator RestoreCardPresentation(Card cardData)
        {
            var cardView = CreateBattleCardView(cardData);
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

            var sequence = DOTween.Sequence();
            
            sequence.Append(view.PlayResolveFadePresentation());
            sequence.Append(PlayCardContainerMovePresentation(isMovingDown: false));
            
            yield return sequence.WaitForCompletion();
            Destroy(view.gameObject);
        }

        private Tween PlayCardContainerMovePresentation(bool isMovingDown)
        {
            cardContainer.DOKill(); 
            
            float targetY = isMovingDown ? originalCardContainerY + processingCardContainerOffsetY : originalCardContainerY;
            float duration = isMovingDown ? cardContainerMoveDownDuration : cardContainerMoveUpDuration;
            Ease ease = isMovingDown ? cardContainerMoveDownEase : cardContainerMoveUpEase;
            
            return cardContainer.DOAnchorPosY(targetY, duration).SetEase(ease);
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
                
                view.SetBaseLayoutTransform(targetPos, targetAngle, Vector3.one);

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
            cardView.Initialize(random, eventBus, presentationManager);
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
            float targetAngle;
            if (totalCount == 2)
            {
                targetAngle = cardIndex == 0 ? maxCardAngle / 2 : -maxCardAngle / 2;
            }
            else
            {
                targetAngle = Mathf.Lerp(maxCardAngle, -maxCardAngle, layoutProgress);
            }

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
                processingCardView.SetLayoutTransform(processingCardPosition, Vector3.zero, processingCardScale);

                sequence.Append(processingCardView.PlayTriggerFadePresentation());
                sequence.Join(PlayCardContainerMovePresentation(isMovingDown: true));
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

                processingCardView.SetBaseLayoutTransform(processingCardPosition, Vector3.zero, processingCardScale);
                
                sequence.Append(processingCardView.PlayProcessMoveToLayoutTransform());
                sequence.Join(PlayCardSortPresentation(cardViews, processSortMoveDuration, processSortRotateDuration, processSortMoveEase, processSortRotateEase));
                sequence.Join(PlayCardContainerMovePresentation(isMovingDown: true));
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
        [Header("Editor Testing - Card Lifecycle")]
        [Tooltip("테스트할 손패(Hand) 안의 CardView를 연결하세요.")]
        [SerializeField] private BattleCardView testCardView;

        [ContextMenu("Test - Open Hand Deck")]
        public void TestOpenHandDeck()
        {
            if (!Application.isPlaying) return;
            StartCoroutine(DelayedTestRoutine(OpenHandDeckPresentation()));
            isHandDeckOpened = true;
        }

        [ContextMenu("Test - Close Hand Deck")]
        public void TestCloseHandDeck()
        {
            if (!Application.isPlaying) return;
            StartCoroutine(DelayedTestRoutine(CloseHandDeckPresentation()));
            isHandDeckOpened = false;
        }

        [ContextMenu("Test - Full Sequence (Use from Hand)")]
        public void TestFullSequenceFromHand()
        {
            if (!Application.isPlaying || testCardView == null) return;
            StartCoroutine(DelayedTestRoutine(TestFullSequenceRoutine(isTriggering: false)));
        }

        [ContextMenu("Test - Full Sequence (Trigger from Void)")]
        public void TestFullSequenceTrigger()
        {
            if (!Application.isPlaying || testCardView == null) return;
            StartCoroutine(DelayedTestRoutine(TestFullSequenceRoutine(isTriggering: true)));
        }

        private IEnumerator TestFullSequenceRoutine(bool isTriggering)
        {
            bool wasInHand = cardViews.Contains(testCardView);
            int originalIndex = testCardView.transform.GetSiblingIndex();
            
            if (wasInHand) cardViews.Remove(testCardView);
            testCardView.transform.SetParent(processingCardContainer);

            var enterSeq = DOTween.Sequence();

            if (isTriggering)
            {
                testCardView.SetLayoutTransform(processingCardPosition, Vector3.zero, processingCardScale);
                enterSeq.Append(testCardView.PlayTriggerFadePresentation());
                enterSeq.Join(PlayCardContainerMovePresentation(isMovingDown: true));
            }
            else
            {
                testCardView.SetBaseLayoutTransform(processingCardPosition, Vector3.zero, processingCardScale);
                enterSeq.Append(testCardView.PlayProcessMoveToLayoutTransform());
                enterSeq.Join(PlayCardSortPresentation(cardViews, processSortMoveDuration, processSortRotateDuration, processSortMoveEase, processSortRotateEase));
                enterSeq.Join(PlayCardContainerMovePresentation(isMovingDown: true));
            }

            yield return enterSeq.WaitForCompletion();

            yield return new WaitForSeconds(0.7f);

            var resolveSeq = DOTween.Sequence();
            
            resolveSeq.Append(testCardView.PlayResolveFadePresentation());
            resolveSeq.Append(PlayCardContainerMovePresentation(isMovingDown: false));
            
            yield return resolveSeq.WaitForCompletion();

            if (wasInHand)
            {
                cardViews.Insert(Mathf.Min(originalIndex, cardViews.Count), testCardView);
            }
            testCardView.transform.SetParent(cardContainer);
            
            testCardView.PlayFadePresentation(0f, Ease.Linear, isFadeIn: true);
            
            testCardView.SetBaseLayoutTransform(Vector3.zero, Vector3.zero, Vector3.one);
            yield return PlayCardSortPresentation(cardViews, cancelSortMoveDuration, cancelSortRotateDuration, cancelSortMoveEase, cancelSortRotateEase).WaitForCompletion();
        }

        private IEnumerator DelayedTestRoutine(IEnumerator targetPresentation)
        {
            yield return new WaitForSeconds(0.4f); 
            yield return StartCoroutine(targetPresentation);
        }
#endif
    }
}