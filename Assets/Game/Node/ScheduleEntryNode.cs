using System;
using UnityEngine;

public class ScheduleEntryNode : Node
{
    public ScheduleEntryNode(Guid skeletonId, Action<Node, Player> OnMoveRequest) : base(OnMoveRequest, skeletonId)
    {
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);
    }
}