using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.IncidentNodeView
{
    public class IncidentChoiceButton : MonoBehaviour 
    {
        [Header("Behaviour")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private CanvasGroup canvasGroup;
        
        [Header("Presenation")]
        [SerializeField] private float moveDuration;
        [SerializeField] private float moveDistance;
        [SerializeField] private float appearDuration;
        [SerializeField] private Ease moveEase;
        [SerializeField] private Ease appearEase;

        private Action onPressed;

        public void Initiate(DeterminedIncidentChoice choice, Action onPressed)
        {
            Unactive();

            mainText.text = choice.Description;
            this.onPressed = onPressed;
            
            gameObject.SetActive(true);
        }

        public Tween PlayAppearPresentation()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Join(rectTransform.DOAnchorPosX(moveDistance, moveDuration).SetEase(moveEase).From(true));
            sequence.Join(canvasGroup.DOFade(1, appearDuration).SetEase(appearEase).From(0));

            return sequence;
        }

        public void Unactive()
        {
            onPressed = null;
            gameObject.SetActive(false);
        }

        public void OnPressed()
        {
            if (onPressed == null) return;

            var actionToInvoke = onPressed;
            Unactive();

            actionToInvoke.Invoke();
        }
    }
}