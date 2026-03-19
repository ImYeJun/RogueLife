using System.Collections;
using DG.Tweening;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class HealthUpdatePresentation : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private BattleHealthView battleHealthView;
        [SerializeField] private MentalityView mentalityView;
        [SerializeField] private PlayerStatusView playerStatusView;
        [SerializeField] private PlayerImageView playerImageView;

        public override void OnInitialized()
        {
            if (battleHealthView == null) Debug.LogWarning("[HealthUpdatePresentation/OnInitialized] battleHealthView is not assigned.");
            if (mentalityView == null) Debug.LogWarning("[HealthUpdatePresentation/OnInitialized] mentalityView is not assigned.");
            if (playerStatusView == null) Debug.LogWarning("[HealthUpdatePresentation/OnInitialized] playerStatusView is not assigned.");
            if (playerImageView == null) Debug.LogWarning("[HealthUpdatePresentation/OnInitialized] playerImageView is not assigned.");

            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus.Subscribe<PlayerHurt>(OnPlayerHurt);
            eventBus.Subscribe<PlayerHealed>(OnPlayerHealed);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus?.Unsubscribe<PlayerHurt>(OnPlayerHurt);
            eventBus?.Unsubscribe<PlayerHealed>(OnPlayerHealed);
        }

        private void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            battleHealthView.DrawViewInstant(payload.Health);
            mentalityView.DrawViewInstant(payload.Health);
            playerStatusView.DrawViewInstant(payload.Health);
        }

        private void OnPlayerHurt(PlayerHurt payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHurt, HurtPresentationRoutine(payload.Health));
        }

        private void OnPlayerHealed(PlayerHealed payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHealed, HealPresentationRoutine(payload.Health));
        }

        private IEnumerator HurtPresentationRoutine(IReadOnlyHealth health)
        {
            Sequence mainSeq = DOTween.Sequence();

            mainSeq.Join(battleHealthView.GetUpdateHealthTween(health));
            mainSeq.Join(mentalityView.GetUpdateMentalityTween(health));
            mainSeq.Join(playerStatusView.GetUpdateSliderTween(health));
            mainSeq.Join(playerImageView.GetHurtEffectTween());

            yield return mainSeq.WaitForCompletion();
        }

        private IEnumerator HealPresentationRoutine(IReadOnlyHealth health)
        {
            Sequence mainSeq = DOTween.Sequence();

            mainSeq.Join(battleHealthView.GetUpdateHealthTween(health));
            mainSeq.Join(mentalityView.GetUpdateMentalityTween(health));
            mainSeq.Join(playerStatusView.GetUpdateSliderTween(health));

            yield return mainSeq.WaitForCompletion();
        }
    }
}