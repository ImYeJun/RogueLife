using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Text;

namespace View.BattleView
{
    public class CardDescriptionView : MonoBehaviour
    {
        [Header("Behaviour")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private CanvasGroup canvasGroup; 

        [Header("Settings")]
        [SerializeField] private float floatingDistance = 300f;
        [SerializeField] private float appearOffset = 20f; 
        
        [SerializeField] private Vector2 padding = new Vector2(40f, 40f); 
        [SerializeField] private bool useFixedWidth = true;
        [SerializeField] private float fixedWidth = 400f;

        [Header("Tween Presentation")]
        [SerializeField] private float fadeDuration = 0.15f;
        [SerializeField] private float moveDuration = 0.2f;
        [SerializeField] private Ease moveEase = Ease.OutBack;
        [SerializeField] private Ease fadeEase = Ease.OutQuad;

        private Tween currentTween;

        public Func<string, BattleStatusEffectData> GetStatusEffectData;

        public void Initialize(Func<string, BattleStatusEffectData> getStatusEffectData)
        {
            GetStatusEffectData = getStatusEffectData;
        }

        public void Focus(BattleCardView focusedCardView)
        {
            currentTween?.Kill();

            var targetPosition = focusedCardView.gameObject.transform.localPosition + Vector3.up * floatingDistance;
            
            StringBuilder sb = new StringBuilder();
            sb.Append(focusedCardView.Card.CurrentDescription);
            sb.Append("\n<size=70%>");
            foreach (var statusEffectId in focusedCardView.Card.CurrentRelatedStatusEffectIds)
            {
                var data = GetStatusEffectData.Invoke(statusEffectId);
                if (data != null)
                {
                    sb.Append($"({data.Name})").Append("\n").Append(data.Description).Append("\n"); 
                }
            }
            sb.Append("</size>");
            
            description.text = sb.ToString();
            description.ForceMeshUpdate();

            float finalHeight;

            if (useFixedWidth)
            {
                Vector2 textSize = description.GetPreferredValues(fixedWidth - padding.x, float.PositiveInfinity);
                finalHeight = textSize.y + padding.y;
                rectTransform.sizeDelta = new Vector2(fixedWidth, finalHeight);
            }
            else
            {
                Vector2 textSize = description.GetPreferredValues();
                finalHeight = textSize.y + padding.y;
                rectTransform.sizeDelta = new Vector2(textSize.x + padding.x, finalHeight);
            }

            if (finalHeight > 500f)
            {
                float difference = finalHeight - 500f;
                targetPosition += new Vector3(0, difference, 0);
            }

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