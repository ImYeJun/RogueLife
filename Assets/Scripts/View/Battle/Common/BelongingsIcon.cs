using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View.Core;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public class BelongingsIcon : ViewBehaviour<IBattleViewEvent>, IPointerEnterHandler, IPointerExitHandler
    {
        private BattleBelongings belongings = null;

        [Header("Behaviour")]
        [SerializeField] private Image image;
        [SerializeField] private GameObject descriptionPanel;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [Header("Focusing Presentation")]
        [SerializeField] private float focusingPresentationDuration;
        [SerializeField] private float focusingScale;
        [SerializeField] private Ease focusingPresentationEase;

        [Header("Executed Presentation")]
        [SerializeField] private float executedPresentationDuration;
        [SerializeField] private Ease executedPresentationEase;
        [SerializeField] private Vector3 punchAmount;
        [SerializeField] private int punchVibrato;
        [SerializeField] private float punchElasticity;
        [SerializeField] float executePresentationHoldDuration = 0.3f;

        private Tween currentFocusingTween;

        public void Initialize(BattleBelongings belongings)
        {
            descriptionPanel.SetActive(false);
            this.belongings = belongings;
            
            nameText.text = belongings.Name;
            descriptionText.text = belongings.Description;
            image.sprite = belongings.Image;
            
            nameText.ForceMeshUpdate();
            descriptionText.ForceMeshUpdate();

            float nameHeight = nameText.GetPreferredValues(belongings.Name, nameText.rectTransform.rect.width, 0).y;
            float descHeight = descriptionText.GetPreferredValues(belongings.Description, descriptionText.rectTransform.rect.width, 0).y;

            float panelHeight = 0;
            panelHeight += nameHeight + nameText.margin.y + nameText.margin.w;
            panelHeight += descHeight + descriptionText.margin.y + descriptionText.margin.w;

            RectTransform panelRect = descriptionPanel.GetComponent<RectTransform>();
            if (panelRect != null)
            {
                panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, panelHeight);
            }
        }

        public override void OnInitialized()
        {
            eventBus.Subscribe<BelongingsEffectExecuted>(OnBelongingsEffectExecuted);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<BelongingsEffectExecuted>(OnBelongingsEffectExecuted);
        }

        private void OnBelongingsEffectExecuted(BelongingsEffectExecuted payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BeloningsEffectExecuted, PlayExecutePresentation());
        }

        private IEnumerator PlayExecutePresentation()
        {
            if (this == null) yield break;

            yield return image.transform.DOPunchScale(punchAmount, executedPresentationDuration, punchVibrato, punchElasticity)
                .SetEase(executedPresentationEase)
                .SetLink(gameObject)
                .WaitForCompletion();

            if (this == null) yield break;
            yield return new WaitForSeconds(executePresentationHoldDuration);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (belongings is null) { return; }

            descriptionPanel.SetActive(true);
            currentFocusingTween?.Kill();
            currentFocusingTween = image.transform.DOScale(focusingScale, CalculateFocusingDuration(transform.localScale.x)).SetEase(focusingPresentationEase);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (belongings is null) { return; }

            descriptionPanel.SetActive(false);
            currentFocusingTween?.Kill();
            currentFocusingTween = image.transform.DOScale(1, CalculateFocusingDuration(transform.localScale.x)).SetEase(focusingPresentationEase);
        }

        private float CalculateFocusingDuration(float currentScale)
        {
            float originalDelta = Mathf.Abs(focusingScale - 1);
            float currentDelta = Mathf.Abs(focusingScale - currentScale);

            float ratio = originalDelta == 0 ? 0 : currentDelta/originalDelta;

            return focusingPresentationDuration * ratio;
        }
    }
}