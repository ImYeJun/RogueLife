using System;
using System.Collections.Generic;

public class TransactionNode : Node
{
    private Dictionary<TransactionChoiceOrder, TransactionChoiceEntity> choices = new Dictionary<TransactionChoiceOrder, TransactionChoiceEntity>();

    public TransactionNode(Guid skeletonId) : base(skeletonId)
    {
    }

    public override void OnEnter(FieldContext context, INodeFlowHandler flowHandler, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, flowHandler, scheduleHistory);

        RequestNextNodeSelection();
        return;
        
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
        choice.OnSelected(context, this);

        if (choice.IsInstantEffect)
        {
            RequestNextNodeSelection();
        }
    }

    public override void OnExit(Node nextNode)
    {
        scheduleHistory.RecordTransaction();
        RecordBelongingsEquipping();

        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}