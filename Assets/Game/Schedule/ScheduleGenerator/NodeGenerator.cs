using System;
using System.Collections.Generic;
using System.Reflection;

public class NodeGenerator
{
    private IEngageBattle battleSystem;
    private EnemyDataSlot bossDataSlot;
    private Node exitNode;

    public NodeGenerator(IEngageBattle battleSystem)
    {
        this.battleSystem = battleSystem;
    }

    public EnemyDataSlot BossDataSlot { get => bossDataSlot; }
    public Node ExitNode { get => exitNode; }

    public void Reset() { 
        bossDataSlot = null;
        exitNode = null;
    }

    public Node Generate(Guid skeletonId, NodeSkeleton nodeSkeleton, ScheduleData data, Action<Node, Player, FieldContext> onMoveRequest, Action onScheduleEnd)
    {
        switch (nodeSkeleton.FixedType)
        {
            case NodeType.ENTRY:
                return new ScheduleEntryNode(skeletonId, onMoveRequest);
            case NodeType.BATTLE:
                return new BattleNode(skeletonId, onMoveRequest, battleSystem, null); //TODO determine the enemy and StartPhaseCount by ScheduleData
            case NodeType.INCIDENT:
                return new IncidentNode(skeletonId, onMoveRequest, new IncidentData()); //TODO determine the incident data by ScheduleData
            case NodeType.TRANSACTION:
                return new TransactionNode(skeletonId, onMoveRequest);
            case NodeType.BOSS:
                if (bossDataSlot != null) { throw new InvalidOperationException("More than two boss nodes cannot be existed in a schedule."); }
                bossDataSlot = new EnemyDataSlot(data.BossData);
                return new BattleNode(skeletonId, onMoveRequest, battleSystem, new List<EnemyDataSlot>{ bossDataSlot });
            case NodeType.EXIT:
                if (exitNode != null) { throw new InvalidOperationException("More than two exit nodes cannot be existed in a schedule."); }
                var node = new ScheduleExitNode(skeletonId, onMoveRequest, onScheduleEnd);
                exitNode = node;
                return node;
            default:
                throw new ArgumentException($"Invalid Node Type to genearte : {nodeSkeleton.FixedType}");
        }
    }
}