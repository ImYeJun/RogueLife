using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class PlayerImageView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private RectTransform wholeBody;
        [SerializeField] private RectTransform heatlhBar;
        [SerializeField] private Sprite idleImage;
        [SerializeField] private Sprite hurtImage;
        [SerializeField] private Sprite walkImage;
        private Image image; 

        [Header("Player Shake Settings")]
        [SerializeField] private float hurtEffectDuration = 0.3f;
        [SerializeField] private float shakeStrength = 15f;
        [SerializeField] private int shakeVibrato = 10;     

        [Header("Health Bar Shake Settings")]
        [SerializeField] private float healthBarShakeDuration = 0.2f;
        [SerializeField] private float healthBarShakeStrength = 7f; 
        [SerializeField] private int healthBarShakeVibrato = 10;

        public override void OnInitialized()
        {
            image = GetComponent<Image>();
            image.sprite = idleImage;
        }
        
        public override void OnDestroy() { }

        public Tween GetHurtEffectTween()
        {
            Sequence seq = DOTween.Sequence();

            seq.AppendCallback(() => SetHurtView());

            if (wholeBody != null)
            {
                seq.Append(wholeBody.DOShakeAnchorPos(hurtEffectDuration, shakeStrength, shakeVibrato));
            }
            else
            {
                Debug.LogWarning("[PlayerImageView/GetHurtEffectTween] wholeBody is null.");
            }

            seq.AppendCallback(() => SetIdleView());

            if (heatlhBar != null)
            {
                seq.Append(heatlhBar.DOShakeAnchorPos(healthBarShakeDuration, healthBarShakeStrength, healthBarShakeVibrato));
            }
            else
            {
                Debug.LogWarning("[PlayerImageView/GetHurtEffectTween] heatlhBar is null.");
            }

            return seq;
        }

        public void SetIdleView() { image.sprite = idleImage; }
        public void SetHurtView() { image.sprite = hurtImage; }
        public void SetWalkView() { image.sprite = walkImage; }
    }
}