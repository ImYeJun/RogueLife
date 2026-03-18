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

    public override void OnEnter(FieldContext context, IScheduleRouter router, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, router, scheduleHistory);

        context.Health.OnMentalBreakDown += OnPlayerMentalBroken;

        var determiendChoices = entity.DetermineEffect(context);

        router.RequestIncidentSelection(entity.Data, determiendChoices);
    }

    public void OnChoiceSettled(DeterminedIncidentChoice selectedChoice)
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