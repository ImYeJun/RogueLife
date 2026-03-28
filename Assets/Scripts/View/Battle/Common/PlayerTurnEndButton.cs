using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System.Collections;
using DG.Tweening;
using System;

namespace View.BattleView
{
    public class PlayerTurnEndButton : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>
    {
        [SerializeField] private Vector3 shownPosition;
        [SerializeField] private Vector3 disappearPosition;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        [Header("Show Presentation")]
        [SerializeField] private float showMoveDuration;
        [SerializeField] private float showFadeDuration;
        [SerializeField] private Ease showMoveEase;
        [SerializeField] private Ease showFadeEase;

        [Header("Disappear Presentation")]
        [SerializeField] private float disappearMoveDuration;
        [SerializeField] private float disappearFadeDuration;
        [SerializeField] private Ease disappearMoveEase;
        [SerializeField] private Ease disappearFadeEase;

        private Tween currentTween;
        
        private bool isPlayerTurnActive = false;
        private bool isCardTargeting = false;
        private bool isCurrentlyVisible = false;

        private void Awake() 
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            SetInteractable(false);

            canvasGroup.alpha = 0;
            rectTransform.anchoredPosition = disappearPosition;
        }

        public override void OnInitialized()
        {
            eventBus.Subscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus.Subscribe<PlayerTurnEnding>(OnPlayerTurnEnding);
            eventBus.Subscribe<BattleEnded>(OnBattleEnded);
        }
        
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus?.Unsubscribe<PlayerTurnEnding>(OnPlayerTurnEnding);
            eventBus?.Unsubscribe<BattleEnded>(OnBattleEnded);
        }

        public void SetTargetingState(bool targeting)
        {
            isCardTargeting = targeting;
            EvaluateVisibility();
        }

        private void OnPlayerTurnStarted(PlayerTurnStarted payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnStarted_TurnEndButtonShow, ShowPresentation());
        }

        private void OnPlayerTurnEnding(PlayerTurnEnding payload)
        {
            isPlayerTurnActive = false;
            EvaluateVisibility();
        }

        private void OnBattleEnded(BattleEnded ended)
        {
            isPlayerTurnActive = false;
            EvaluateVisibility();
        }

        private void EvaluateVisibility()
        {
            bool shouldBeVisible = isPlayerTurnActive && !isCardTargeting;

            if (shouldBeVisible && !isCurrentlyVisible)
            {
                isCurrentlyVisible = true;
                PlayShow();
            }
            else if (!shouldBeVisible && isCurrentlyVisible)
            {
                isCurrentlyVisible = false;
                PlayDisappear();
            }
        }

        private IEnumerator ShowPresentation()
        {
            isPlayerTurnActive = true; 
            EvaluateVisibility();
            
            if (currentTween != null)
            {
                yield return currentTween.WaitForCompletion();
            }
        }

        public Sequence PlayShow()
        {
            SetInteractable(true);
            currentTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(shownPosition, showMoveDuration).SetEase(showMoveEase));
            sequence.Join(canvasGroup.DOFade(1, showFadeDuration).SetEase(showFadeEase));
            
            currentTween = sequence;
            return sequence;
        }

        private IEnumerator DisappearPresentation()
        {
            EvaluateVisibility();
            if (currentTween != null)
            {
                yield return currentTween.WaitForCompletion();
            }
        }

        public Sequence PlayDisappear()
        {
            SetInteractable(false);
            currentTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(disappearPosition, disappearMoveDuration).SetEase(disappearMoveEase));
            sequence.Join(canvasGroup.DOFade(0, disappearFadeDuration).SetEase(disappearFadeEase));
            
            currentTween = sequence;
            return sequence;
        }

        private void SetInteractable(bool value)
        {
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }

        public void OnPressed()
        {
            if (!isPlayerTurnActive) return;
            
            isPlayerTurnActive = false;
            EvaluateVisibility();
            
            commander.EndPlayerTurn();
        }

#if UNITY_EDITOR
        [ContextMenu("Play Show Presentation")]
        public void TestPlayShow()
        {
            isPlayerTurnActive = true;
            isCardTargeting = false;
            StartCoroutine(DelayPlay(ShowPresentation()));
        }

        [ContextMenu("Play Disappear Presentation")]
        public void TestPlayDisappear()
        {
            isPlayerTurnActive = false;
            StartCoroutine(DelayPlay(DisappearPresentation()));
        }

        private IEnumerator DelayPlay(IEnumerator presentation)
        {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(presentation);
        }
#endif
    }
}