using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public class BattleCardView : MonoBehaviour, IPointerClickHandler
    {
        [Header("Settings")]
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

            transform.localPosition = baseLocalPosition;
            transform.localEulerAngles = baseLocalEulerAngles;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onCardClicked?.Invoke(this);
        }

        public void Focus()
        {
            Vector3 tiltDirection = transform.localRotation * Vector3.up;

            transform.localPosition = baseLocalPosition + (tiltDirection * popUpDistance);
        }

        public void Unfocus()
        {
            transform.localPosition = baseLocalPosition;
        }
    }
}