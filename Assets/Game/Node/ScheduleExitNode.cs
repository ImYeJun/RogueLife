using System;
using UnityEngine;

public class ScheduleExitNode : Node
{
    public Action OnScheduleEnd;

    public ScheduleExitNode(Guid skeletonId, Action<Node, Player, FieldContext> OnMoveRequest, Action OnScheduleEnd) : base(OnMoveRequest, skeletonId)
    {
        this.OnScheduleEnd = OnScheduleEnd;
    }

    public override void OnEnter(Player player, FieldContext context, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(player, context, scheduleHistory);

        scheduleHistory.RemainMentalityOnExit = player.Health.CurrentMentality;
        OnScheduleEnd.Invoke();
    }
}