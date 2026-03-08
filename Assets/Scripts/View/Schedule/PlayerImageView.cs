using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class PlayerImageView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Sprite idleImage;
        [SerializeField] private Sprite hurtImage;
        [SerializeField] private Sprite walkImage;
        private Image image; 

        [SerializeField] private float hurtEffectDuration;
        private Coroutine hurtEffectCoroutine;
        

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

        private void OnPlayerHurt(PlayerHurt hurt)
        {
            if (hurtEffectCoroutine is not null) StopCoroutine(hurtEffectCoroutine);
            
            hurtEffectCoroutine = StartCoroutine(HurtEffect());
        }

        private IEnumerator HurtEffect()
        {
            SetHurtView();
            yield return new WaitForSeconds(hurtEffectDuration);
            SetIdleView();

            hurtEffectCoroutine = null;
        }

        public void SetIdleView() { image.sprite = idleImage; }
        public void SetHurtView() { image.sprite = hurtImage; }
        public void SetWalkView() { image.sprite = walkImage; }
    }
}
