using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class BattleHealthView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Image battleHealthSlider;
        [SerializeField] private TextMeshProUGUI battleHealthText;

        [Header("Tween Settings")]
        [SerializeField] private Ease fillEase = Ease.OutQuad;
        [SerializeField] private Ease textEase = Ease.OutQuad;

        private float currentDisplayedHealth;

        public override void OnInitialized() { }
        public override void OnDestroy() { }

        public void DrawViewInstant(IReadOnlyHealth health)
        {
            if (health == null)
            {
                Debug.LogWarning("[BattleHealthView/DrawViewInstant] health is null.");
                return;
            }

            battleHealthSlider.fillAmount = health.NormalizedBattleHealth;
            currentDisplayedHealth = health.CurrentBattleHealth;
            battleHealthText.text = $"{health.CurrentBattleHealth}/{health.MaxBattleHealth}";
        }

        public Tween GetUpdateHealthTween(int newHealth, int currentMaxHealth, float duration, float offsetDuration)
        {
            var sequence = DOTween.Sequence();
            float normalizedBattleHealth = currentMaxHealth == 0 ? 0 : (float)newHealth/currentMaxHealth;
            
            sequence.Join(battleHealthSlider.DOFillAmount(normalizedBattleHealth, duration).SetEase(fillEase));

            int targetHealth = newHealth;
            int maxHealth = currentMaxHealth;
            
            sequence.Join(DOTween.To(() => currentDisplayedHealth, x => 
            {
                currentDisplayedHealth = x;
                battleHealthText.text = $"{Mathf.RoundToInt(currentDisplayedHealth)}/{maxHealth}";
            }, targetHealth, duration + offsetDuration).SetEase(textEase));

            return sequence;
        }
    }
}