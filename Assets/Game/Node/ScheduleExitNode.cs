using System;
using UnityEngine;

public class ScheduleExitNode : Node
{
    public event Action OnScheduleEnd;

    public ScheduleExitNode(Guid skeletonId, Action<Node> OnMoveRequest, Action OnScheduleEnd) : base(OnMoveRequest, skeletonId)
    {
        this.OnScheduleEnd += OnScheduleEnd;
    }

    public override void OnEnter()
    {
        OnScheduleEnd.Invoke();
    }
}