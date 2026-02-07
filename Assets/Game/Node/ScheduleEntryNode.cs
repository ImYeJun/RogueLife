using System;
using UnityEngine;

public class ScheduleEntryNode : Node
{
    public ScheduleEntryNode(Guid skeletonId, Action<Node> OnMoveRequest) : base(OnMoveRequest, skeletonId)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //TODO : 일정 테마 선택 기능 구현
    }
}