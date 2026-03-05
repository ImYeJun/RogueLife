using System;
using UnityEngine;

public class ScheduleExitNode : Node
{
    public Action OnScheduleEnd;

    public ScheduleExitNode(Guid skeletonId, Action OnScheduleEnd) : base(skeletonId)
    {
        this.OnScheduleEnd = OnScheduleEnd;
    }

    public override void OnEnter(FieldContext context, INodeFlowHandler flowHandler, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, flowHandler, scheduleHistory);

        scheduleHistory.RemainMentalityOnExit = context.Health.CurrentMentality;
        OnScheduleEnd.Invoke();
    }
}