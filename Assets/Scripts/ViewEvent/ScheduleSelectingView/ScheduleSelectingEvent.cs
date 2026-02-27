using System;
using System.Collections.Generic;
using Unity.Mathematics;
using ViewEvent.Core;

namespace ViewEvent.ScheduleSelecting
{
    public interface IScheduleSelectingEvent : IViewEvent { }

    public readonly struct ReadyToSelectSchedule : IScheduleSelectingEvent
    {
        private readonly List<ScheduleData> availableScheduleData;
        private readonly int currentStartCount;

        public ReadyToSelectSchedule(List<ScheduleData> availableScheduleData, int currentStartCount)
        {
            this.availableScheduleData = availableScheduleData;
            this.currentStartCount = currentStartCount;
        }

        public List<ScheduleData> AvailableScheduleData => availableScheduleData;
        public int CurrentStartCount => currentStartCount;
    }

    public readonly struct ScheduleSettled : IScheduleSelectingEvent {}
}