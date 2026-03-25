using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.ScheduleView.CollectionUpdateView
{
    public class ItemView : MonoBehaviour, IPointerClickHandler{
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float popUpDuration;
        [SerializeField] private float popUpAmount;
        [SerializeField] private Ease popUpEase;
        private Tween currentPopUpTween;
        public Action OnClicked;


        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnDisable()
        {
            currentPopUpTween?.Kill();
        }

        protected void PopUp()
        {
            currentPopUpTween?.Kill(true);
            rectTransform.localScale = new Vector3(1, 1, 1);
            currentPopUpTween = rectTransform.DOAnchorPosY(-popUpAmount, popUpDuration).SetEase(popUpEase).From(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}