using System;
using UnityEngine;

public class ScheduleExitNode : Node
{
    public Action OnScheduleEnd;

    public ScheduleExitNode(Guid skeletonId, Action<Node, FieldContext> OnMoveRequest, Action OnScheduleEnd) : base(OnMoveRequest, skeletonId)
    {
        this.OnScheduleEnd = OnScheduleEnd;
    }

    public override void OnEnter(FieldContext context, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, scheduleHistory);

        scheduleHistory.RemainMentalityOnExit = context.Health.CurrentMentality;
        OnScheduleEnd.Invoke();
    }
}