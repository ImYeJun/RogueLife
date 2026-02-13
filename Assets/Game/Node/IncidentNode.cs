using System;
using System.Collections.Generic;
using UnityEngine;

public class IncidentNode : Node
{    
    private List<Choice> choices;

    public IncidentNode(Guid skeletonId, Action<Node, Player> OnMoveRequest) : base(OnMoveRequest, skeletonId)
    {
    }

    public List<Choice> Choices { get => choices; }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);
        //TODO : choices에 따라 선택지 UI 띄우기
    }
}