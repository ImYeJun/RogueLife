using System;
using TMPro;
using UnityEngine;
using DG.Tweening; // 💡 DOTween 추가!

namespace View.BattleView
{
    public class CardDescriptionView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private CanvasGroup canvasGroup; 

        [Header("Settings")]
        [SerializeField] private float floatingDistance = 300f;
        [SerializeField] private float appearOffset = 20f; 

        [Header("Tween Presentation")]
        [SerializeField] private float fadeDuration = 0.15f;
        [SerializeField] private float moveDuration = 0.2f;
        [SerializeField] private Ease moveEase = Ease.OutBack;
        [SerializeField] private Ease fadeEase = Ease.OutQuad;

        private Tween currentTween;

        public void Focus(BattleCardView focusedCardView)
        {
            currentTween?.Kill();

            var targetPosition = focusedCardView.gameObject.transform.localPosition + Vector3.up * floatingDistance;
            
            description.text = focusedCardView.Card.CurrentDescription;
            gameObject.SetActive(true);

            transform.localPosition = targetPosition - new Vector3(0, appearOffset, 0);
            canvasGroup.alpha = 0f;

            var sequence = DOTween.Sequence();
            sequence.Join(transform.DOLocalMove(targetPosition, moveDuration).SetEase(moveEase));
            sequence.Join(canvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEase));

            currentTween = sequence;
        }

        public void Unfocus()
        {
            currentTween?.Kill();

            if (!gameObject.activeSelf) return;

            var targetPosition = transform.localPosition - new Vector3(0, appearOffset, 0);

            var sequence = DOTween.Sequence();
            sequence.Join(transform.DOLocalMove(targetPosition, moveDuration).SetEase(Ease.InBack)); 
            sequence.Join(canvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEase));
            
            sequence.OnComplete(() => gameObject.SetActive(false));

            currentTween = sequence;
        }
    }
}