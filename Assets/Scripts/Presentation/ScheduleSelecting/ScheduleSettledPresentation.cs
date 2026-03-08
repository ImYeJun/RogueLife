using System.Collections;
using DG.Tweening;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleSelecting;

namespace View.ScheduleSelecting
{
    public class ScheduleSettledPresentation : ViewBehaviour<IScheduleSelectingEvent>
    {
        [Header("Behaviour")]
        [SerializeField] private GameObject foreground;
        [SerializeField] private Material fadeInMaterial;
        [SerializeField] private float fadeInDuration = 1.0f;
        [SerializeField] private Ease fadeInEasingType = Ease.OutQuart;

        private Tween currentTween;
        
        private static readonly int CenterID = Shader.PropertyToID("_Center");
        private static readonly int RadiusID = Shader.PropertyToID("_Radius");

        public override void OnInitialized()
        {
            foreground.SetActive(false);
            eventBus.Subscribe<ScheduleSettled>(OnScheduleSettled);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleSettled>(OnScheduleSettled);
        }

        public void OnScheduleSettled(ScheduleSettled payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.ScheduleSettled_FadeIn, FadeInPresentation(payload.SelectPos));
        }

        public IEnumerator FadeInPresentation(Vector2 selectPos)
        {
            currentTween?.Kill();

            fadeInMaterial.SetVector(CenterID, new Vector4(selectPos.x, selectPos.y, 0, 0));
            fadeInMaterial.SetFloat(RadiusID, 2.5f);

            foreground.SetActive(true);

            currentTween = fadeInMaterial.DOFloat(0f, RadiusID, fadeInDuration)
                .SetEase(fadeInEasingType);

            yield return currentTween.WaitForCompletion();
        }

#if UNITY_EDITOR
        [Header("Test")]
        [SerializeField] private Vector2 testClickPos = new Vector2(0.5f, 0.5f);

        [ContextMenu("Play FadeIn Presentation (Center)")]
        public void TestFadeInCenter()
        {
            presentationManager.Enqueue(0, PresentationPriority.ScheduleSettled_FadeIn, FadeInPresentation(new Vector2(0.5f, 0.5f)));
        }

        [ContextMenu("Play FadeIn Presentation (Custom Pos)")]
        public void TestFadeInCustom()
        {
            presentationManager.Enqueue(0, PresentationPriority.ScheduleSettled_FadeIn, FadeInPresentation(testClickPos));
        }
#endif
    }
}