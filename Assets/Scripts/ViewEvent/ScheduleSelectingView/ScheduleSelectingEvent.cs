using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using ViewEvent.Core;

namespace ViewEvent.ScheduleSelecting
{
    public interface IScheduleSelectingEvent : IViewEvent { }

    public readonly struct ReadyToSelectSchedule : IScheduleSelectingEvent
    {
        private readonly int sequenceId;
        private readonly List<ScheduleData> availableScheduleData;
        private readonly int currentStartCount;

        public ReadyToSelectSchedule(int sequenceId, List<ScheduleData> availableScheduleData, int currentStartCount)
        {
            this.sequenceId = sequenceId;
            this.availableScheduleData = availableScheduleData;
            this.currentStartCount = currentStartCount;
        }
        public int SequenceId => sequenceId;

        public List<ScheduleData> AvailableScheduleData => availableScheduleData;
        public int CurrentStartCount => currentStartCount;
    }

    public readonly struct ScheduleSettled : IScheduleSelectingEvent
    {
        private readonly int sequenceId;
        private readonly Vector2 selectPos;

        public ScheduleSettled(int sequenceId, Vector2 selectPos)
        {
            this.sequenceId = sequenceId;
            this.selectPos = selectPos;
        }
        public int SequenceId => sequenceId;
        public Vector2 SelectPos => selectPos;
    }

    public readonly struct WentToBed : IScheduleSelectingEvent
    {
        private readonly int sequenceId;

        public WentToBed(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId => sequenceId;
    }
}