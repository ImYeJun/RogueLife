using System;
using UnityEngine;

public class ScheduleExitNode : Node
{
    public event Action OnScheduleEnd;

    public override void OnEnter()
    {
        OnScheduleEnd.Invoke();
    }
}