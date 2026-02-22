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

        context.RecordEncounterEnemyForChoiceEngageBattleEffect = scheduleHistory.RecordEncounterEnemy; //! SHIT HACK, Refactor it!
        context.RequestNextNodeSelectionForChoiceEngageBattleEffect = RequestNextNodeSelection; //! SHIT HACK, Refactor it!
        context.OnPlayerMentalBrokenForChoiceEngageBattleEffect = OnPlayerMentalBroken; //! SHIT HACK, Refactor it!
        context.OnExitForChoiceEngageBattleEffect = ShitOnExit; //! SHIT HACK, Refactor it!
    }

    public void OnChoiceSettled(DeterminedIncidentChoiceData selectedChoice)
    {  
        selectedChoice.OnSelected(context);
        
        if (!context.HasEngagedBattleByChoiceEngageBattleEffect) { //! SHIT HACK, Refactor it!
            RequestNextNodeSelection();
        }
    }

    protected override void OnExit(Node nextNode)
    {
        scheduleHistory.RecordEncounterIncident(data);
        RecordBelongingsEquipping();

        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
    public void ShitOnExit(Node nextNode)
    {
        OnExit(nextNode);
    }
}