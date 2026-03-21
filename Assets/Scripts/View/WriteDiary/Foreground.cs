using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.WriteDiaryView;

namespace View.WriteDiaryView
{
    public class Foreground : ViewBehaviour<IWriteDiaryViewEvent>
    {
        [SerializeField] private Image foreground;
        [SerializeField] private float fadeDuration;
        [SerializeField] private Ease fadeEase;

        public override void OnInitialized()
        {
            foreground.gameObject.SetActive(true);
            eventBus.Subscribe<DiaryWritten>(OnDiaryWritten);
            eventBus.Subscribe<ReturnToMainMenuRequested>(OnReturnToMainMenuRequested);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<DiaryWritten>(OnDiaryWritten);
            eventBus?.Unsubscribe<ReturnToMainMenuRequested>(OnReturnToMainMenuRequested);
        }

        private void OnDiaryWritten(DiaryWritten payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.DiaryWritten_FadeIn, FadePresentation(false));
        }
        private void OnReturnToMainMenuRequested(ReturnToMainMenuRequested payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.ReturnToMainMenuRequested_FadeOut, FadePresentation(true));
        }

        private IEnumerator FadePresentation(bool isFadeOut = true)
        {
            foreground.gameObject.SetActive(true);
            float from = isFadeOut ? 0 : 1;
            float to = isFadeOut ? 1 : 0;

            yield return foreground.DOFade(to, fadeDuration).From(from).SetEase(fadeEase).WaitForCompletion();

            foreground.gameObject.SetActive(false);
        }
    }
}
