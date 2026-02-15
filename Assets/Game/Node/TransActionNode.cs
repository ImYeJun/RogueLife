using System;
using System.Collections.Generic;

public class TransactionNode : Node
{
    private Dictionary<TransactionChoiceOrder, TransactionChoiceData> choices = new Dictionary<TransactionChoiceOrder, TransactionChoiceData>();

    public TransactionNode(Guid skeletonId, Action<Node, FieldContext> OnMoveRequest) : base(OnMoveRequest, skeletonId)
    {
    }

    public override void OnEnter(FieldContext context, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, scheduleHistory);
        context.Health.OnMentalBreakDown += OnPlayerMentalBroken;
        
        foreach (TransactionChoiceOrder order in Enum.GetValues(typeof(TransactionChoiceOrder)))
        {
            if (context.TransactionChoiceDatabase.TryGetRandomData(context, order, out var choiceData))
            {
                choices[order] = choiceData;
            }
        }

        //TODO choices을 OnChoiceSettled와 함께 UI로 보내기
    }

    public void OnChoiceSettled(TransactionChoiceOrder order)
    {
        if (!choices.ContainsKey(order)) { throw new InvalidOperationException($"[TransactionNode] there's no choice data for {order}"); }

        var choice = choices[order];
        choice.OnSelected(context);

        RequestNextNodeSelection();
    }

    protected override void OnExit(Node nextNode)
    {
        scheduleHistory.RecordTransaction();
        RecordBelongingsEquipping();

        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}