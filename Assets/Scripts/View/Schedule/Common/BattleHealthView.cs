using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class BattleHealthView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Image battleHealthSlider;
        [SerializeField] private TextMeshProUGUI battleHealthText;

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
            DrawView(payload.Health);
        }
        
        public void OnPlayerHurt(PlayerHurt payload)
        {
            DrawView(payload.Health);
        }

        public void OnPlayerHealed(PlayerHealed payload)
        {
            DrawView(payload.Health);
        }
        
        private void DrawView(IReadOnlyHealth health)
        {
            battleHealthSlider.fillAmount = health.NormalizedBattleHealth;
            battleHealthText.text = $"{health.CurrentBattleHealth}/{health.MaxBattleHealth}";
        }
    }
}