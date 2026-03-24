using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class PlayerStatusView : MonoBehaviour
    {
        [SerializeField] private Image mentalitySlider;

        [SerializeField] private Image portrait;
        [SerializeField] private Sprite idleSprite;
        [SerializeField] private Sprite hurtSprite;

        [Header("Tween Settings")]
        [SerializeField] private Ease fillEase = Ease.OutQuad;

        public void Awake()
        {
            SetIdlePortrait();
        }

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

        public void SetIdlePortrait()
        {
            portrait.sprite = idleSprite;
        }
        public void SetHurtPortrait()
        {
            portrait.sprite = hurtSprite;
        }
    }
}