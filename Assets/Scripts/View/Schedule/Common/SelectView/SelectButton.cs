using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Global;

namespace View.ScheduleView
{
    [RequireComponent(typeof(ButtonJuice))]
    public abstract class SelectButton : MonoBehaviour, IPointerClickHandler 
    {
        [Header("Base Behaviour")]
        [SerializeField] private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Action onPressed;

        [Header("Presentation")]
        [SerializeField] private float fadeDuration = 0.1f;
        [SerializeField] private Ease fadeEase = Ease.InOutCubic;
        [SerializeField] private float moveDuration = 0.3f;
        [SerializeField] private float moveDistance = 10f;
        [SerializeField] private Ease moveEase = Ease.OutBack;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        protected void InitAction(Action onPressed)
        {
            this.onPressed = onPressed;
        }

        public void SetVisible(bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
            SetInteractable(value);
        }
        public void SetInteractable(bool value)
        {
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onPressed?.Invoke();
        }
        
        public Tween PlayShowPresentation()
        {
            SetVisible(true);
            
            var sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(1, fadeDuration).From(0).SetEase(fadeEase).SetLink(gameObject));
            sequence.Join(rectTransform.DOAnchorPosX(moveDistance, moveDuration).From(true).SetEase(moveEase).SetLink(gameObject));

            return sequence;
        }

    }
}