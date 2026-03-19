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

        public override void OnInitialized() { }
        public override void OnDestroy() { }

        public void DrawViewInstant(IReadOnlyHealth health)
        {
            if (health == null)
            {
                Debug.LogWarning("[MentalityView/DrawViewInstant] health is null.");
                return;
            }

            mentalitySlider.fillAmount = health.NomarlizedMentality;
            currentDisplayedMentality = health.CurrentMentality;
            mentalityText.text = $"{health.CurrentMentality}/{health.MaxMentality}";
        }

        public Tween GetUpdateMentalityTween(IReadOnlyHealth health)
        {
            if (health == null)
            {
                Debug.LogWarning("[MentalityView/GetUpdateMentalityTween] health is null.");
                return null;
            }

            var sequence = DOTween.Sequence();
            
            sequence.Join(mentalitySlider.DOFillAmount(health.NomarlizedMentality, fillDuration).SetEase(fillEase));

            int targetMentality = health.CurrentMentality;
            int maxMentality = health.MaxMentality;
            
            sequence.Join(DOTween.To(() => currentDisplayedMentality, x => 
            {
                currentDisplayedMentality = x;
                mentalityText.text = $"{Mathf.RoundToInt(currentDisplayedMentality)}/{maxMentality}";
            }, targetMentality, fillDuration + offsetDuration).SetEase(fillEase));

            return sequence;
        }
    }
}