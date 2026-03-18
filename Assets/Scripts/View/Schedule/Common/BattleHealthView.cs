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

        public override void OnInitialized()
        {
            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus.Subscribe<PlayerHurt>(OnPlayerHurt);
            eventBus.Subscribe<PlayerHealed>(OnPlayerHealed);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus?.Unsubscribe<PlayerHurt>(OnPlayerHurt);
            eventBus?.Unsubscribe<PlayerHealed>(OnPlayerHealed);
        }

        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            DrawViewInstant(payload.Health);
        }
        
        public void OnPlayerHurt(PlayerHurt payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHurt, UpdateHealthRoutine(payload.Health));
        }

        public void OnPlayerHealed(PlayerHealed payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHealed, UpdateHealthRoutine(payload.Health));
        }
        
        private void DrawViewInstant(IReadOnlyHealth health)
        {
            battleHealthSlider.fillAmount = health.NormalizedBattleHealth;
            currentDisplayedHealth = health.CurrentBattleHealth;
            battleHealthText.text = $"{health.CurrentBattleHealth}/{health.MaxBattleHealth}";
        }

        private IEnumerator UpdateHealthRoutine(IReadOnlyHealth health)
        {
            var sequence = DOTween.Sequence();
            
            sequence.Join(battleHealthSlider.DOFillAmount(health.NormalizedBattleHealth, fillDuration).SetEase(fillEase));

            int targetHealth = health.CurrentBattleHealth;
            int maxHealth = health.MaxBattleHealth;
            
            sequence.Join(DOTween.To(() => currentDisplayedHealth, x => 
            {
                currentDisplayedHealth = x;
                battleHealthText.text = $"{Mathf.RoundToInt(currentDisplayedHealth)}/{maxHealth}";
            }, targetHealth, fillDuration + offsetDuration).SetEase(fillEase));

            yield return sequence.WaitForCompletion();
        }
    }
}