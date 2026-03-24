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
        private Vector3 originalPosition;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private CanvasGroup canvasGroup; 

        [Header("Settings")]
        [SerializeField] private float floatingDistance = 300f;
        
        [SerializeField] private Vector2 padding = new Vector2(40f, 40f); 
        [SerializeField] private bool useFixedWidth = true;
        [SerializeField] private float fixedWidth = 400f;

        [Header("Tween Presentation")]
        [SerializeField] private float fadeDuration = 0.15f;
        [SerializeField] private Ease fadeEase = Ease.OutQuad;

        private Tween currentTween;

        public Func<string, BattleStatusEffectData> GetStatusEffectData;

        public void Initialize(Func<string, BattleStatusEffectData> getStatusEffectData)
        {
            GetStatusEffectData = getStatusEffectData;
            originalPosition = rectTransform.anchoredPosition;
        }

        public void Focus(BattleCardView focusedCardView)
        {
            currentTween?.Kill();

            var targetPosition = originalPosition;
            
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

            if (finalHeight > 400f)
            {
                float difference = finalHeight - 400f;
                targetPosition += new Vector3(0, difference, 0);
            }

            transform.localPosition = targetPosition;
            
            gameObject.SetActive(true);
            canvasGroup.alpha = 0f;

            currentTween = canvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEase);
        }

        public void Unfocus()
        {
            currentTween?.Kill();

            if (!gameObject.activeSelf) return;

            currentTween = canvasGroup.DOFade(0f, fadeDuration)
                .SetEase(fadeEase)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}