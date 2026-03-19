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
        
        public Tween GetUpdateSliderTween(int newHealth, int currentMaxHealth, float duration)
        {
            float nomarlizedMentality = currentMaxHealth == 0 ? 0 : (float)newHealth/currentMaxHealth;
            return mentalitySlider.DOFillAmount(nomarlizedMentality, duration).SetEase(fillEase);
        }
    }
}