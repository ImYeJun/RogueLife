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

        private bool isViewVisible = false;
        private int? pendingTurnValue = null;

        public override void OnInitialized()
        {
            rectTransform.anchoredPosition = disappearingUpDestination;
            isViewVisible = false;

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
            rectTransform?.DOKill();
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
            
            if (pendingTurnValue.HasValue)
            {
                DrawPhaseText(pendingTurnValue.Value);
                pendingTurnValue = null;
            }
            isViewVisible = true;
            
            yield return rectTransform.DOAnchorPos(showingPosition, showingDownDuration).SetEase(showingDownEasingType).WaitForCompletion();
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
            
            yield return rectTransform.DOAnchorPos(disappearingUpDestination, disappearingUpDuration).SetEase(disappearingUpEasingType).WaitForCompletion();
        }

        public void OnPhaseIncreased(PhaseIncreased payload)
        {
            HandlePhaseUpdate(payload.CurrentPhase);
        }
        
        public void OnPhaseDecreased(PhaseDecreased payload)
        {
            HandlePhaseUpdate(payload.CurrentPhase);
        }

        public void OnInitialPhaseSettled(InitialPhaseSettled payload)
        {
            phase = payload.Phase;
            HandlePhaseUpdate(phase.ReaminTurn);
        }

        private void HandlePhaseUpdate(int currentPhase)
        {
            if (isViewVisible)
            {
                DrawPhaseText(currentPhase);
            }
            else
            {
                pendingTurnValue = currentPhase;
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