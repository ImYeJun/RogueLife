using System;
using UnityEngine;

public class ScheduleEntryNode : Node
{
    public ScheduleEntryNode(Guid skeletonId, Action<Node, Player, FieldContext> OnMoveRequest) : base(OnMoveRequest, skeletonId)
    {
    }

    public override void OnEnter(Player player, FieldContext context, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(player, context, scheduleHistory);
        RequestNextNodeSelection();
    }
}