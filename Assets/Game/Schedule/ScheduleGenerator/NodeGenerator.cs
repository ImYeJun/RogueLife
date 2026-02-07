using System;
using System.Reflection;

public class NodeGenerator
{
    private BattleSystem battleSystem;

    public NodeGenerator(BattleSystem battleSystem)
    {
        this.battleSystem = battleSystem;
    }

    public Node Generate(Guid skeletonId, NodeSkeleton nodeSkeleton, Action<Node> onMoveRequest, Action onScheduleEnd)
    {
        switch (nodeSkeleton.FixedType)
        {
            case NodeType.ENTRY:
                return new ScheduleEntryNode(skeletonId, onMoveRequest);
            case NodeType.BATTLE:
                return new BattleNode(skeletonId, battleSystem, onMoveRequest, null); //TODO determine the enemy by ScheduleData
            case NodeType.INCIDENT:
                return new IncidentNode(skeletonId, onMoveRequest);
            case NodeType.TRANSACION:
                return new TransactionNode(skeletonId, onMoveRequest);
            case NodeType.EXIT:
                return new ScheduleExitNode(skeletonId, onMoveRequest, onScheduleEnd);
            default:
                throw new ArgumentException($"Invalid Node Type to genearte : {nodeSkeleton.FixedType}");
        }
    }
}