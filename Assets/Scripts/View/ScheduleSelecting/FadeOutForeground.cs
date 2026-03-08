using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleSelecting;

namespace View.ScheduleSelecting
{
    public class FadeOutForeground : ViewBehaviour<IScheduleSelectingEvent>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeOutDuration;
        [SerializeField] private Ease fadeOutEasingType;
        private Tween tween;

        public override void OnInitialized()
        {
            canvasGroup.alpha = 1;
            canvasGroup.gameObject.SetActive(true);

            eventBus.Subscribe<ReadyToSelectSchedule>(OnReadyToSelectSchedule);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ReadyToSelectSchedule>(OnReadyToSelectSchedule);
        }

        public void OnReadyToSelectSchedule(ReadyToSelectSchedule payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.ReadyToSelectSchedule_FadeOut, FadeOutPresentation());
        }

        public IEnumerator FadeOutPresentation()
        {
            tween?.Kill();
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 1;

            tween = canvasGroup.DOFade(0, fadeOutDuration).SetEase(fadeOutEasingType);
            yield return tween.WaitForCompletion();

            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(false);
        } 
    }
}
