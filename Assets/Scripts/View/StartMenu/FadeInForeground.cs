using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.StartMenu;

namespace View.StartMenu
{
    public class FadeInForeground : ViewBehaviour<IStartMenuViewEvent>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private Ease fadeInEasingType;
        private Tween tween;
        
        public override void OnInitialized()
        {
            canvasGroup.gameObject.SetActive(false);

            eventBus.Subscribe<ReadyToStartGame>(OnReadyToStartGame);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ReadyToStartGame>(OnReadyToStartGame);
        }

        public void OnReadyToStartGame(ReadyToStartGame payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.ReadyToStartGame_FadeIn, FadeInPresentation());
        }

        public IEnumerator FadeInPresentation()
        {
            tween?.Kill();

            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            tween = canvasGroup.DOFade(1f, fadeInDuration).SetEase(fadeInEasingType);

            yield return tween.WaitForCompletion();

            canvasGroup.alpha = 1f;
        }
    }
}
