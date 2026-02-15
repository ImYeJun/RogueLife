using System;
using System.Collections.Generic;
using UnityEngine;

public class IncidentNode : Node
{    
    private IncidentData data;
    private List<IIncidentChoiceData> choices;

    public IncidentNode(Guid skeletonId, Action<Node, FieldContext> OnMoveRequest, IncidentData data) : base(OnMoveRequest, skeletonId)
    {
        this.data = data;
        choices = data.Choices;
    }

    public List<IIncidentChoiceData> Choices { get => choices; }

    public override void OnEnter(FieldContext context, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, scheduleHistory);

        context.Health.OnMentalBreakDown += OnPlayerMentalBroken;

        //TODO : choices에 따라 선택지 UI 띄우기

        context.RecordEncounterEnemyForChoiceEngageBattleEffect = scheduleHistory.RecordEncounterEnemy; //! SHIT HACK, Refactor it!
        context.RequestNextNodeSelectionForChoiceEngageBattleEffect = RequestNextNodeSelection; //! SHIT HACK, Refactor it!
        context.OnPlayerMentalBrokenForChoiceEngageBattleEffect = OnPlayerMentalBroken; //! SHIT HACK, Refactor it!
    }

    public void OnChoiceSettled(IIncidentChoiceData selectedcChoice)
    {  
        selectedcChoice.OnSelected(context);

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
}