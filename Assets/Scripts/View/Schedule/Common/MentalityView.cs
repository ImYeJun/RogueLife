using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class MentalityView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Image mentalitySlider;
        [SerializeField] private TextMeshProUGUI mentalityText;

        [Header("Tween Settings")]
        [SerializeField] private float fillDuration = 0.3f;
        [SerializeField] private Ease fillEase = Ease.OutQuad;
        [SerializeField] private float offsetDuration = 0.2f;

        private float currentDisplayedMentality;

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
            DrawViewInstant(payload.Health);
        }
        
        public void OnPlayerHurt(PlayerHurt payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHurt, UpdateMentalityRoutine(payload.Health));
        }

        public void OnPlayerHealed(PlayerHealed payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHealed, UpdateMentalityRoutine(payload.Health));
        }
        
        private void DrawViewInstant(IReadOnlyHealth health)
        {
            mentalitySlider.fillAmount = health.NomarlizedMentality;
            currentDisplayedMentality = health.CurrentMentality;
            mentalityText.text = $"{health.CurrentMentality}/{health.MaxMentality}";
        }

        private IEnumerator UpdateMentalityRoutine(IReadOnlyHealth health)
        {
            var sequence = DOTween.Sequence();
            
            sequence.Join(mentalitySlider.DOFillAmount(health.NomarlizedMentality, fillDuration).SetEase(fillEase));

            int targetMentality = health.CurrentMentality;
            int maxMentality = health.MaxMentality;
            
            sequence.Join(DOTween.To(() => currentDisplayedMentality, x => 
            {
                currentDisplayedMentality = x;
                mentalityText.text = $"{Mathf.RoundToInt(currentDisplayedMentality)}/{maxMentality}";
            }, targetMentality, fillDuration + offsetDuration).SetEase(fillEase));

            yield return sequence.WaitForCompletion();
        }
    }
}