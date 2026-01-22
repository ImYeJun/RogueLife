using System;
using UnityEngine;

public class ScheduleExitNode : Node
{
    public event Action OnScheduleEnd;

    public ScheduleExitNode(Action<Node> OnMoveRequest, Action OnScheduleEnd) : base(OnMoveRequest)
    {
        this.OnScheduleEnd += OnScheduleEnd;
    }

    public override void OnEnter()
    {
        OnScheduleEnd.Invoke();
    }
}