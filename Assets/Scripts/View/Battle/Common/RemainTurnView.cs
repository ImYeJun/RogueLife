using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using TMPro;
using System;
using UnityEngine.Serialization;
using System.Collections;
using DG.Tweening;

namespace View.BattleView
{
    public class RemainTurnView : ViewBehaviour<IBattleViewEvent>
    {
        private IReadOnlyBattlePhase phase;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField, FormerlySerializedAs("remainPhaseText")] private TextMeshProUGUI remainTurnText;

        [Header("Disappearing Up Presentation")]
        [SerializeField] Vector2 disappearingUpDestination;
        [SerializeField] float disappearingUpDuration;
        [SerializeField] Ease disappearingUpEasingType;

        [Header("Showing Down Presentation")]
        [SerializeField] Vector2 showingPosition;
        [SerializeField] float showingDownDuration;
        [SerializeField] Ease showingDownEasingType;

        [Header("Phase Update Presentation")]
        [SerializeField] float textPunchDuration = 0.3f;
        [SerializeField] Vector3 textPunchScale = new Vector3(0.3f, 0.3f, 0f);
        [SerializeField] float waitTimeBeforeHide = 0.5f;

        private Tween currentTween;
        private bool isViewVisible = false;

        public override void OnInitialized()
        {
            rectTransform.anchoredPosition = disappearingUpDestination;
            isViewVisible = false;
            remainTurnText.transform.localScale = Vector3.one;

            eventBus.Subscribe<InitialPhaseSettled>(OnInitialPhaseSettled);
            eventBus.Subscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus.Subscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
            eventBus.Subscribe<EnemyTurnStarted>(OnEnemyTurnStarted);
            eventBus.Subscribe<EnemyTurnEnded>(OnEnemyTurnEnded);
            eventBus.Subscribe<PhaseIncreased>(OnPhaseIncreased);
            eventBus.Subscribe<PhaseDecreased>(OnPhaseDecreased);
        }

        public override void OnDestroy()
        {
            KillActiveTweens();

            eventBus?.Unsubscribe<InitialPhaseSettled>(OnInitialPhaseSettled);
            eventBus?.Unsubscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus?.Unsubscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
            eventBus?.Unsubscribe<EnemyTurnStarted>(OnEnemyTurnStarted);
            eventBus?.Unsubscribe<EnemyTurnEnded>(OnEnemyTurnEnded);
            eventBus?.Unsubscribe<PhaseIncreased>(OnPhaseIncreased);
            eventBus?.Unsubscribe<PhaseDecreased>(OnPhaseDecreased);
        }

        private void KillActiveTweens()
        {
            currentTween?.Kill();
        }

        public void OnInitialPhaseSettled(InitialPhaseSettled payload)
        {
            phase = payload.Phase;
            DrawPhaseText(phase.ReaminTurn);
        }

        private void OnPlayerTurnStarted(PlayerTurnStarted payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnStarted_TurnViewShowingDown, ShowingDownPresentation());
        }
        
        private void OnEnemyTurnStarted(EnemyTurnStarted payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyTurnStarted_TurnViewShowingDown, ShowingDownPresentation());
        }
        
        private IEnumerator ShowingDownPresentation()
        {
            KillActiveTweens(); 
            
            isViewVisible = true;
            
            currentTween = rectTransform.DOAnchorPos(showingPosition, showingDownDuration).SetEase(showingDownEasingType);
            yield return currentTween.WaitForCompletion();
        }

        private void OnPlayerTurnEnded(PlayerTurnEnded payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnEnded_TurnViewDisappearingUp, MoveUpPresentation());
        }
        
        private void OnEnemyTurnEnded(EnemyTurnEnded payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyTurnEnded_TurnViewDisappearingUp, MoveUpPresentation());
        }
        
        private IEnumerator MoveUpPresentation()
        {
            KillActiveTweens();
            
            isViewVisible = false;
            
            currentTween = rectTransform.DOAnchorPos(disappearingUpDestination, disappearingUpDuration).SetEase(disappearingUpEasingType);
            yield return currentTween.WaitForCompletion();
        }

        public void OnPhaseIncreased(PhaseIncreased payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PhaseIncreased_UpdateView, PhaseUpdatePresentation(payload.CurrentPhase));
        }
        
        public void OnPhaseDecreased(PhaseDecreased payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PhaseDecreased_UpdateView, PhaseUpdatePresentation(payload.CurrentPhase));
        }

        private IEnumerator PhaseUpdatePresentation(int currentPhase)
        {
            bool wasHidden = !isViewVisible;

            if (wasHidden)
            {
                yield return StartCoroutine(ShowingDownPresentation());
            }

            DrawPhaseText(currentPhase);
            
            remainTurnText.transform.DOKill(true);
            yield return remainTurnText.transform.DOPunchScale(textPunchScale, textPunchDuration, 5, 1).WaitForCompletion();

            if (wasHidden)
            {
                yield return new WaitForSeconds(waitTimeBeforeHide);
                yield return StartCoroutine(MoveUpPresentation());
            }
        }
        
        private void DrawPhaseText(int currentPhase)
        {
            remainTurnText.text = currentPhase.ToString();
        }

#if UNITY_EDITOR
        [ContextMenu("Test Showing Down Presentation")]
        public void TestShowingDownPresentation()
        {
            StartCoroutine(ShowingDownPresentation());
        }

        [ContextMenu("Test Move Up Presentation")]
        public void TestMoveUpPresentation()
        {
            StartCoroutine(MoveUpPresentation());
        }
#endif
    }
}