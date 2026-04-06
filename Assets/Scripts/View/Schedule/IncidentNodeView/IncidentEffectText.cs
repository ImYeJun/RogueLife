using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using View.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.IncidentNodeView
{
    public class IncidentEffectText : ViewBehaviour<IScheduleViewEvent>
    {
        private TextMeshProUGUI text;

        [Header("Animation Settings")]
        [SerializeField] private float popUpDuration = 0.5f;
        [SerializeField] private float popUpDistance;
        [SerializeField] private Ease popUpMoveEase = Ease.OutBack;
        [SerializeField] private Ease popUpFadeEase = Ease.InOutCubic;
        
        [Header("Duration Settings")]
        [SerializeField] private float baseAppearDuration = 1.3f;
        [Tooltip("2줄 이상일 때, 1줄당 추가될 시간")]
        [SerializeField] private float durationPerLine = 0.3f; 
        
        [SerializeField] private float disappearDurtaion = 0.3f;
        [SerializeField] private Ease disappearFadeEase = Ease.InOutCubic;

        public override void OnInitialized()
        {
            text = GetComponent<TextMeshProUGUI>();
            gameObject.SetActive(false);

            eventBus.Subscribe<IncidentSelcted>(OnIncidentSelcted);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<IncidentSelcted>(OnIncidentSelcted);
        }

        private void OnIncidentSelcted(IncidentSelcted payload)
        {
            if (string.IsNullOrEmpty(payload.EffectDescription)) { return; }

            text.text = payload.EffectDescription;
            
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.IncidentSelected, PlayPresentation(), () =>
            {
                text?.gameObject?.SetActive(false);
            });
        }

        private IEnumerator PlayPresentation()
        {
            gameObject.SetActive(true);
            
            text.ForceMeshUpdate();
            int lineCount = text.textInfo.lineCount;

            float finalAppearDuration = baseAppearDuration;
            if (lineCount >= 2)
            {
                finalAppearDuration += (lineCount * durationPerLine);
            }

            var popUpPresentation = DOTween.Sequence();
            popUpPresentation.SetLink(gameObject);
            popUpPresentation.Join(text.DOFade(1, popUpDuration).From(0).SetEase(popUpFadeEase));
            popUpPresentation.Join(text.GetComponent<RectTransform>().DOAnchorPosY(popUpDistance, popUpDuration).From(true).SetEase(popUpMoveEase));

            yield return popUpPresentation.WaitForCompletion();
            
            yield return new WaitForSeconds(finalAppearDuration);

            yield return text.DOFade(0, disappearDurtaion).From(1).SetEase(disappearFadeEase).WaitForCompletion();
        }

        [ContextMenu("Test Play Presentation")]
        private void TestPlayPresentation()
        {
            StartCoroutine(TestPlay());
        }

        private IEnumerator TestPlay() 
        {
            yield return new WaitForSeconds(0.5f); 
            StartCoroutine(PlayPresentation());
        }
    }
}