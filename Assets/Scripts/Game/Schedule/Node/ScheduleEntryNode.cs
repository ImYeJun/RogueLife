using System;
using UnityEngine;

public class ScheduleEntryNode : Node
{
    public ScheduleEntryNode(Guid skeletonId) : base(skeletonId)
    {
    }
    
    public override void OnEnter(FieldContext context, IScheduleRouter flowHandler, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, flowHandler, scheduleHistory);

        context.Health.OnMentalBreakDown += OnPlayerMentalBroken;

        RequestNextNodeSelection();
    }

    public override void OnExit(Node nextNode)
    {
        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}