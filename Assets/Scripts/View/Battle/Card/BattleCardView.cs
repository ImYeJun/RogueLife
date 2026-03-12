using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public class BattleCardView : MonoBehaviour, IPointerClickHandler
    {
        [Header("Settings")]
        [SerializeField] public RectTransform rectTransform;
        [SerializeField] private float popUpDistance = 0.5f; 
        [SerializeField] private SharedCardView sharedCardView;

        private Action<BattleCardView> onCardClicked; 
        private Vector3 baseLocalPosition;
        private Vector3 baseLocalEulerAngles;

        public Card Card => sharedCardView.Card;

        public void Initialize(Card card, Action<BattleCardView> onCardClicked)
        {
            this.onCardClicked = onCardClicked;
            sharedCardView.SetCard(card);
        }

        public void SetLayoutTransform(Vector3 targetLocalPos, Vector3 targetLocalAngles)
        {
            baseLocalPosition = targetLocalPos;
            baseLocalEulerAngles = targetLocalAngles;

            rectTransform.anchoredPosition = baseLocalPosition;
            transform.localEulerAngles = baseLocalEulerAngles;
        }
        public void SetBaseLayoutTransform(Vector3 basePos, Vector3 baseAngles)
        {
            baseLocalPosition = basePos;
            baseLocalEulerAngles = baseAngles;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onCardClicked?.Invoke(this);
        }

        public void Focus()
        {
            Vector3 tiltDirection = transform.localRotation * Vector3.up;

            rectTransform.anchoredPosition = baseLocalPosition + (tiltDirection * popUpDistance);
        }

        public void Unfocus()
        {
            rectTransform.anchoredPosition = baseLocalPosition;
        }
    }
}