using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.ScheduleView.CollectionUpdateView
{
    public class ItemView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float popUpDuration;
        [SerializeField] private float popUpAmount;
        [SerializeField] private Ease popUpEase;
        [SerializeField] private float pointerEnterDuration;
        [SerializeField] private float pointerEnterScaleAmount;
        [SerializeField] private Ease pointerEnterEase;
        [SerializeField] private float pointerExitDuration;
        [SerializeField] private Ease pointerExitEase;

        private Tween currentPopUpTween;
        private Tween currentScaleTween;
        private bool isPointerEnter;
        public Action OnClicked;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnDisable()
        {
            isPointerEnter = false;
            currentPopUpTween?.Kill();
            currentScaleTween?.Kill();
        }

        protected void PopUp()
        {
            currentPopUpTween?.Kill(true);
            currentScaleTween?.Kill();
            rectTransform.localScale = new Vector3(1, 1, 1);
            currentPopUpTween = rectTransform.DOAnchorPosY(-popUpAmount, popUpDuration).SetEase(popUpEase).From(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerEnter = true;
            currentScaleTween?.Kill();
            currentScaleTween = rectTransform.DOScale(pointerEnterScaleAmount, CalculateDynamicDuration(transform.localScale.x, pointerEnterScaleAmount, pointerEnterDuration)).SetEase(pointerEnterEase);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerEnter = false;
            currentScaleTween?.Kill();
            currentScaleTween = rectTransform.DOScale(1, CalculateDynamicDuration(transform.localScale.x, 1, pointerExitDuration)).SetEase(pointerExitEase);
        }

        private float CalculateDynamicDuration(float currentScale, float targetScale, float baseDuration)
        {
            float totalDistance = Mathf.Abs(pointerEnterScaleAmount - 1f); 
            if (totalDistance <= 0f) return 0f; 
            float currentDistance = Mathf.Abs(targetScale - currentScale); 
            
            float ratio = currentDistance / totalDistance; 
            return baseDuration * ratio;
        }
    }
}