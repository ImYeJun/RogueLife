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
        }

        public override void OnDestroy()
        {
            eventBus.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }

        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            mentalitySlider.fillAmount = payload.Health.NomarlizedMentality;
        }
    }
}
