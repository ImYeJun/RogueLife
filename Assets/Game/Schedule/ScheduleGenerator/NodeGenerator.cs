using System;
using System.Collections.Generic;
using System.Reflection;

public class NodeGenerator
{
    private IEngageBattle battleSystem;
    private EnemyDataSlot bossDataSlot;

    public NodeGenerator(IEngageBattle battleSystem)
    {
        this.battleSystem = battleSystem;
    }

    public EnemyDataSlot BossDataSlot { get => bossDataSlot; }

    public void Reset() { bossDataSlot = null; }
    public Node Generate(Guid skeletonId, NodeSkeleton nodeSkeleton, ScheduleData data, Action<Node, Player> onMoveRequest, Action onScheduleEnd)
    {
        switch (nodeSkeleton.FixedType)
        {
            case NodeType.ENTRY:
                return new ScheduleEntryNode(skeletonId, onMoveRequest);
            case NodeType.BATTLE:
                return new BattleNode(skeletonId, battleSystem, onMoveRequest, null); //TODO determine the enemy and StartPhaseCount by ScheduleData
            case NodeType.INCIDENT:
                return new IncidentNode(skeletonId, onMoveRequest);
            case NodeType.TRANSACTION:
                return new TransactionNode(skeletonId, onMoveRequest);
            case NodeType.BOSS:
                bossDataSlot = new EnemyDataSlot(data.BossData);
                return new BattleNode(skeletonId, battleSystem, onMoveRequest, new List<EnemyDataSlot>{ bossDataSlot });
            case NodeType.EXIT:
                return new ScheduleExitNode(skeletonId, onMoveRequest, onScheduleEnd);
            default:
                throw new ArgumentException($"Invalid Node Type to genearte : {nodeSkeleton.FixedType}");
        }
    }
}