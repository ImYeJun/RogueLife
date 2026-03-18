using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class PlayerStatusView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Image mentalitySlider;

        [Header("Tween Settings")]
        [SerializeField] private float fillDuration = 0.3f;
        [SerializeField] private Ease fillEase = Ease.OutQuad;

        public override void OnInitialized()
        {
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

        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            mentalitySlider.fillAmount = payload.Health.NomarlizedMentality;
        }
        
        public void OnPlayerHurt(PlayerHurt payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHurt, UpdateSliderRoutine(payload.Health));
        }
        
        public void OnPlayerHealed(PlayerHealed payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHealed, UpdateSliderRoutine(payload.Health));
        }
        
        private IEnumerator UpdateSliderRoutine(IReadOnlyHealth health)
        {
            yield return mentalitySlider.DOFillAmount(health.NomarlizedMentality, fillDuration).SetEase(fillEase).WaitForCompletion();
        }
    }
}