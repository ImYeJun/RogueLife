using System;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class PlayerStatusView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Image mentalitySlider;

        public override void OnInitialized()
        {
            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus.Subscribe<PlayerHurt>(OnPlayerHurt);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus?.Unsubscribe<PlayerHurt>(OnPlayerHurt);
        }

        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            DrawView(payload.Health);
        }
        public void OnPlayerHurt(PlayerHurt payload)
        {
            DrawView(payload.Health);
        }
        private void DrawView(IReadOnlyHealth health)
        {
            mentalitySlider.fillAmount = health.NomarlizedMentality;
        }
    }
}
