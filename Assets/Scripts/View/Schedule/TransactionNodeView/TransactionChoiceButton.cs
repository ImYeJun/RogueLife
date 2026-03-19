using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.TransactionNodeView
{
    public class TransactionChoiceButton : MonoBehaviour
    {
        [Header("Behaviour")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI subText;
        private Action onPressed;

        [Header("Presenation")] 
        [SerializeField] private float moveDuration;
        [SerializeField] private float moveDistance;
        [SerializeField] private float appearDuration;
        [SerializeField] private Ease moveEase;
        [SerializeField] private Ease appearEase;

        public void Initiate(TransactionChoiceData choiceData, Action onPressed)
        {
            Unactive();

            mainText.text = choiceData.Description;
            subText.text = choiceData.SubDescription;

            this.onPressed = onPressed;
            gameObject.SetActive(true);
        }

        public void Unactive()
        {
            onPressed = null;
            gameObject.SetActive(false);
        }

        public void OnPressed()
        {
            onPressed?.Invoke();
        }

        public Tween PlayAppearPresentation()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Join(rectTransform.DOAnchorPosX(moveDistance, moveDuration).SetEase(moveEase).From(true));
            sequence.Join(canvasGroup.DOFade(1, appearDuration).SetEase(appearEase).From(0));

            return sequence;
        }
        
    }
}