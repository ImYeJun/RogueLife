using System;
using System.Collections.Generic;
using UnityEngine;

public class IncidentNode : Node
{    
    private IncidentData data;
    private List<IncidentChoiceData> choices;

    public IncidentNode(Guid skeletonId, Action<Node, Player, FieldContext> OnMoveRequest, IncidentData data) : base(OnMoveRequest, skeletonId)
    {
        this.data = data;
        choices = data.Choices;
    }

    public List<IncidentChoiceData> Choices { get => choices; }

    public override void OnEnter(Player player, FieldContext context, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(player, context, scheduleHistory);

        player.Health.OnMentalBreakDown += OnPlayerMentalBroken;

        //TODO : choices에 따라 선택지 UI 띄우기
    }

    public void OnChoiceSettled()
    {
        RequestNextNodeSelection();
    }

    protected override void OnExit(Node nextNode)
    {
        scheduleHistory.RecordEncounterIncident(data);
        RecordBelongingsEquipping();

        player.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}