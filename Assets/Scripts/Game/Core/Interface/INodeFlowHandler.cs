using System.Collections.Generic;

public interface IScheduleRouter
{
    public void MoveNode(Node nextNode, FieldContext context, IScheduleRouter scheduleRouter, ScheduleHistory scheduleHistory);
    public void RequestNextNodeSelection(List<Node> nextNodes);
    public void RequestIncidentSelection(IncidentData incidentData, List<DeterminedIncidentChoice> choices);
    public void RequestTransactionSelection(Dictionary<TransactionChoiceOrder, TransactionChoiceData> choices);
    public void RequestBattleTransition();
}