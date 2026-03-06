using System;
using System.Collections.Generic;
using UnityEngine;

public class IncidentNode : Node
{    
    private IncidentEntity entity;

    public IncidentNode(Guid skeletonId, IncidentEntity entity) : base(skeletonId)
    {
        this.entity = entity;
    }

    public override void OnEnter(FieldContext context, INodeFlowHandler flowHandler, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, flowHandler, scheduleHistory);

        RequestNextNodeSelection();
        return;

        context.Health.OnMentalBreakDown += OnPlayerMentalBroken;

        //TODO : choices에 따라 선택지 UI 띄우기
        var determiendChoices = entity.DetermineEffect(context);
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
        scheduleHistory.RecordEncounterIncident(entity.Data);
        RecordBelongingsEquipping();

        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}