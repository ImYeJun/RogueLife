using System;
using System.Reflection;

public class NodeGenerator
{
    private IEngageBattle battleSystem;

    public NodeGenerator(IEngageBattle battleSystem)
    {
        this.battleSystem = battleSystem;
    }

    public Node Generate(Guid skeletonId, NodeSkeleton nodeSkeleton, Action<Node, Player> onMoveRequest, Action onScheduleEnd)
    {
        switch (nodeSkeleton.FixedType)
        {
            case NodeType.ENTRY:
                return new ScheduleEntryNode(skeletonId, onMoveRequest);
            case NodeType.BATTLE:
                return new BattleNode(skeletonId, battleSystem, onMoveRequest, null); //TODO determine the enemy by ScheduleData
            case NodeType.INCIDENT:
                return new IncidentNode(skeletonId, onMoveRequest);
            case NodeType.TRANSACTION:
                return new TransactionNode(skeletonId, onMoveRequest);
            case NodeType.BOSS:
                return new BattleNode(skeletonId, battleSystem, onMoveRequest, null); //TODO determine the boss by ScheduleData
            case NodeType.EXIT:
                return new ScheduleExitNode(skeletonId, onMoveRequest, onScheduleEnd);
            default:
                throw new ArgumentException($"Invalid Node Type to genearte : {nodeSkeleton.FixedType}");
        }
    }
}