using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using View.Core;
using ViewEvent.ScheduleSelecting;

namespace View.ScheduleSelecting
{
    public class ScheduleSelectView : ViewBehaviour<IScheduleSelectingEvent>
    {
        [SerializeField] private List<ScheduleSelectButton> buttons;

        public override void OnDestroy()
        {
            eventBus.Unsubscribe<ReadyToSelectSchedule>(OnReadyToSelectSchedule);
        }

        public override void OnInitialized()
        {
            eventBus.Subscribe<ReadyToSelectSchedule>(OnReadyToSelectSchedule);
        }

        public void OnReadyToSelectSchedule(ReadyToSelectSchedule payload)
        {
            var availableScheduleData = payload.AvailableScheduleData;

            if (availableScheduleData.Count != Constant.SELECING_SCHEUDLE_COUNT)
            {
                UnityEngine.Debug.LogWarning($"[ScheduleSelectView] availableScheduleData does not has {Constant.SELECING_SCHEUDLE_COUNT} schedule data. It has {availableScheduleData.Count}.");
            }

            for (int i = 0 ; i < buttons.Count; i++)
            {
                var button = buttons[i];
                button.SetData(availableScheduleData[i]);
            }
        }
    }
}