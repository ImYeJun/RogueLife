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
        [SerializeField] private float fillDuration = 0.3f;
        [SerializeField] private Ease fillEase = Ease.OutQuad;
        [SerializeField] private float offsetDuration = 0.2f;

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

        public Tween GetUpdateHealthTween(IReadOnlyHealth health)
        {
            if (health == null)
            {
                Debug.LogWarning("[BattleHealthView/GetUpdateHealthTween] health is null.");
                return null;
            }

            var sequence = DOTween.Sequence();
            
            sequence.Join(battleHealthSlider.DOFillAmount(health.NormalizedBattleHealth, fillDuration).SetEase(fillEase));

            int targetHealth = health.CurrentBattleHealth;
            int maxHealth = health.MaxBattleHealth;
            
            sequence.Join(DOTween.To(() => currentDisplayedHealth, x => 
            {
                currentDisplayedHealth = x;
                battleHealthText.text = $"{Mathf.RoundToInt(currentDisplayedHealth)}/{maxHealth}";
            }, targetHealth, fillDuration + offsetDuration).SetEase(fillEase));

            return sequence;
        }
    }
}