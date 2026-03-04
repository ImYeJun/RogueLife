using System;
using UnityEngine;

public class ScheduleEntryNode : Node
{
    public ScheduleEntryNode(Guid skeletonId, Action<Node, FieldContext> OnMoveRequest) : base(OnMoveRequest, skeletonId)
    {
    }
    public override void OnEnter(FieldContext context, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, scheduleHistory);

        context.Health.OnMentalBreakDown += OnPlayerMentalBroken; //! Test

        RequestNextNodeSelection();
    }

     //! Test
    public override void OnExit(Node nextNode)
    {
        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}