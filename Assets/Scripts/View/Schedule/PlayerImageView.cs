using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // 💡 DOTween 추가
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

            eventBus.Subscribe<PlayerHurt>(OnPlayerHurt);
        }
        
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<PlayerHurt>(OnPlayerHurt);
        }

        private void OnPlayerHurt(PlayerHurt payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHurt, HurtEffectRoutine());
        }

        private IEnumerator HurtEffectRoutine()
        {
            SetHurtView();

            Sequence seq = DOTween.Sequence();

            if (wholeBody != null)
            {
                seq.Append(wholeBody.DOShakeAnchorPos(hurtEffectDuration, shakeStrength, shakeVibrato));
            }

            seq.AppendCallback(() => SetIdleView());

            if (heatlhBar != null)
            {
                seq.Append(heatlhBar.DOShakeAnchorPos(healthBarShakeDuration, healthBarShakeStrength, healthBarShakeVibrato));
            }

            yield return seq.WaitForCompletion();
        }

        public void SetIdleView() { image.sprite = idleImage; }
        public void SetHurtView() { image.sprite = hurtImage; }
        public void SetWalkView() { image.sprite = walkImage; }
    }
}