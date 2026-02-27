using System;
using System.Collections.Generic;
using UnityEngine;

public class IncidentNode : Node
{    
    private IncidentData data;

    public IncidentNode(Guid skeletonId, Action<Node, FieldContext> OnMoveRequest, IncidentData data) : base(OnMoveRequest, skeletonId)
    {
        this.data = data;
    }

    public override void OnEnter(FieldContext context, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, scheduleHistory);

        context.Health.OnMentalBreakDown += OnPlayerMentalBroken;

        //TODO : choices에 따라 선택지 UI 띄우기
        var determiendChoices = data.DetermineEffect(context);
    }

    public void OnChoiceSettled(DeterminedIncidentChoiceData selectedChoice)
    {  
        selectedChoice.OnSelected(context, this);
        
        if (selectedChoice.IsInstantEffect)
        {
            RequestNextNodeSelection();
        }
    }

    public override void OnExit(Node nextNode)
    {
        scheduleHistory.RecordEncounterIncident(data);
        RecordBelongingsEquipping();

        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}