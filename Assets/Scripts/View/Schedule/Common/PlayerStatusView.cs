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

        public override void OnInitialized() { }
        public override void OnDestroy() { }

        public void DrawViewInstant(IReadOnlyHealth health)
        {
            if (health == null)
            {
                Debug.LogWarning("[PlayerStatusView/DrawViewInstant] health is null.");
                return;
            }
            mentalitySlider.fillAmount = health.NomarlizedMentality;
        }
        
        public Tween GetUpdateSliderTween(IReadOnlyHealth health)
        {
            if (health == null)
            {
                Debug.LogWarning("[PlayerStatusView/GetUpdateSliderTween] health is null.");
                return null;
            }
            return mentalitySlider.DOFillAmount(health.NomarlizedMentality, fillDuration).SetEase(fillEase);
        }
    }
}