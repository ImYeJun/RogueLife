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

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            SetInteractable(false);

            canvasGroup.alpha = 0;
            rectTransform.anchoredPosition = disappearPosition;
        }

        public override void OnInitialized()
        {
            eventBus.Subscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus.Subscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
            eventBus.Subscribe<BattleEnded>(OnBattleEnded);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus?.Unsubscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
            eventBus?.Unsubscribe<BattleEnded>(OnBattleEnded);
        }

        private void OnPlayerTurnStarted(PlayerTurnStarted payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnStarted_TurnEndButtonShow, ShowPresentation());
        }
        private void OnBattleEnded(BattleEnded ended)
        {
            PlayDisappear();
        }

        private IEnumerator ShowPresentation()
        {
            yield return PlayShow().WaitForCompletion();
        }
        public Sequence PlayShow()
        {
            SetInteractable(true);
            currentTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(shownPosition, showMoveDuration)).SetEase(showMoveEase);
            sequence.Join(canvasGroup.DOFade(1, showFadeDuration)).SetEase(showFadeEase);
            return sequence;
        }

        private void OnPlayerTurnEnded(PlayerTurnEnded payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnEnded_TurnViewDisappearingUp, DisappearPresentation());
        }
        private IEnumerator DisappearPresentation()
        {
            yield return PlayDisappear().WaitForCompletion();
        }
        public Sequence PlayDisappear()
        {
            SetInteractable(false);
            currentTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(disappearPosition, disappearMoveDuration)).SetEase(disappearMoveEase);
            sequence.Join(canvasGroup.DOFade(0, disappearFadeDuration)).SetEase(disappearFadeEase);
            return sequence;
        }

        private void SetInteractable(bool value)
        {
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }

        public void OnPressed()
        {
            commander.EndPlayerTurn();
        }

#if UNITY_EDITOR
        [ContextMenu("Play Show Presentation")]
        public void TestPlayShow()
        {
            StartCoroutine(DelayPlay(ShowPresentation()));
        }

        [ContextMenu("Play Disappear Presentation")]
        public void TestPlayDisappear()
        {
            StartCoroutine(DelayPlay(DisappearPresentation()));
        }

        private IEnumerator DelayPlay(IEnumerator presentation)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(presentation);
        }
#endif
    }
}
