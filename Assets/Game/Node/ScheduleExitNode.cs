using System;
using UnityEngine;

public class ScheduleExitNode : Node
{
    public Action OnScheduleEnd;

    public ScheduleExitNode(Guid skeletonId, Action<Node, Player> OnMoveRequest, Action OnScheduleEnd) : base(OnMoveRequest, skeletonId)
    {
        this.OnScheduleEnd = OnScheduleEnd;
    }

    public override void OnEnter(Player player, ScheduleHistory scheduleHistory)
    {
        base.player = player;

        scheduleHistory.RemainMentalityOnExit = player.Health.CurrentMentality;
        OnScheduleEnd.Invoke();
    }
}