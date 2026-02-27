using System;
using System.Collections.Generic;
using ViewEvent.Core;

namespace ViewEvent.ScheduleSelecting
{
    public interface IScheduleSelectingEvent : IViewEvent { }

    public readonly struct ReadToSelectSchedule : IScheduleSelectingEvent
    {
        private readonly List<ScheduleData> availableScheduleData;
        private readonly int currentStartCount;

        public ReadToSelectSchedule(List<ScheduleData> availableScheduleData, int currentStartCount)
        {
            this.availableScheduleData = availableScheduleData;
            this.currentStartCount = currentStartCount;
        }

        public List<ScheduleData> AvailableScheduleData => availableScheduleData;

        public int CurrentStartCount => currentStartCount;
    }
}