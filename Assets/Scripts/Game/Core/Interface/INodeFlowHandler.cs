using System.Collections.Generic;

public interface INodeFlowHandler
{
    public void MoveNode(Node nextNode, FieldContext context, INodeFlowHandler nodeFlowHandler, ScheduleHistory scheduleHistory);
    public void RequestNextNodeSelection(List<Node> nextNodes);
}