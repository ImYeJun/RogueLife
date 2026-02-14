using System;
using System.Collections.Generic;

public class TransactionNode : Node
{
    private List<Choice> choices;

    public TransactionNode(Guid skeletonId, Action<Node, Player, FieldContext> OnMoveRequest) : base(OnMoveRequest, skeletonId)
    {
    }

    public List<Choice> Choices { get => choices; }

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
        scheduleHistory.RecordTransaction();
        RecordBelongingsEquipping();

        player.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}