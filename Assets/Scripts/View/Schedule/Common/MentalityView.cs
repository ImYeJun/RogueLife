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
        [SerializeField] private Ease fillEase = Ease.OutQuad;
        [SerializeField] private Ease textEase = Ease.OutQuad;
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

        public Tween GetUpdateMentalityTween(int newHealth, int currentMaxHealth, float duration, float offsetDuration)
        {
            var sequence = DOTween.Sequence();
            float nomarlizedMentality = currentMaxHealth == 0 ? 0 : (float)newHealth/currentMaxHealth;

            sequence.Join(mentalitySlider.DOFillAmount(nomarlizedMentality, duration).SetEase(fillEase));

            int targetMentality = newHealth;
            int maxMentality = currentMaxHealth;
            
            sequence.Join(DOTween.To(() => currentDisplayedMentality, x => 
            {
                currentDisplayedMentality = x;
                mentalityText.text = $"{Mathf.RoundToInt(currentDisplayedMentality)}/{maxMentality}";
            }, targetMentality, duration + offsetDuration).SetEase(textEase));

            return sequence;
        }
    }
}