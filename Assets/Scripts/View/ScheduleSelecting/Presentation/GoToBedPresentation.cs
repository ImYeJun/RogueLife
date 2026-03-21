using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleSelecting;

namespace View.ScheduleSelecting
{
    public class GoToBedPresentation : ViewBehaviour<IScheduleSelectingEvent>
    {
        [SerializeField] private Image fadeOutForground;
        [SerializeField] private float fadeOutDuration;
        [SerializeField] private Ease fadeOutEase;

        public override void OnInitialized()
        {
            eventBus.Subscribe<WentToBed>(OnWentToBed);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<WentToBed>(OnWentToBed);
        }

        private void OnWentToBed(WentToBed payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.WentToBed_FadeOut, WentToBedPresentation());
        }

        private IEnumerator WentToBedPresentation()
        {
            fadeOutForground.gameObject.SetActive(true);
            yield return fadeOutForground.DOFade(1, fadeOutDuration).From(0).SetEase(fadeOutEase).WaitForCompletion();
        } 
    }
}
