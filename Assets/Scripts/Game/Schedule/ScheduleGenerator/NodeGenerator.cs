using System;
using System.Collections.Generic;

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

    //TODO Break this method into pieces by Node SubType
    public Node Generate(Random random, Guid skeletonId, NodeSkeleton nodeSkeleton, ScheduleData data, Action onScheduleEnd)
    {
        switch (nodeSkeleton.FixedType)
        {
            case NodeType.ENTRY:
                return new ScheduleEntryNode(skeletonId);
            case NodeType.BATTLE:
                return MaterializeBattleNode(random, data, skeletonId, nodeSkeleton.SpawnEnemyTier);
            case NodeType.INCIDENT:
                return MaterializeIncidentNode(random, data, skeletonId);
            case NodeType.TRANSACTION:
                return new TransactionNode(skeletonId);
            case NodeType.BOSS:
                if (bossDataSlot != null) { throw new InvalidOperationException("More than two boss nodes cannot be existed in a schedule."); }
                bossDataSlot = new EnemyDataSlot(data.BossData);
                return new BattleNode(skeletonId, battleSystem, new List<EnemyDataSlot>{ bossDataSlot });
            case NodeType.EXIT:
                if (exitNode != null) { throw new InvalidOperationException("More than two exit nodes cannot be existed in a schedule."); }
                var node = new ScheduleExitNode(skeletonId, onScheduleEnd);
                exitNode = node;
                return node;
            default:
                throw new ArgumentException($"Invalid Node Type to genearte : {nodeSkeleton.FixedType}");
        }
    }

    private BattleNode MaterializeBattleNode(Random random, ScheduleData data, Guid skeletonId, EnemyTier spawnEnemyTier)
    {
        var availableEnemyData = spawnEnemyTier == EnemyTier.NORMAL ? data.AvailableNormalEnemyData : data.AvailableEliteEnemyData;
        if (availableEnemyData.Count == 0) { throw new InvalidOperationException("[NodeGenerator] availableEnemyData is empty"); }

        var selecetData = availableEnemyData[random.Next(availableEnemyData.Count)];
        var slots = new List<EnemyDataSlot>{new EnemyDataSlot(selecetData)};

        return new BattleNode(skeletonId, battleSystem, slots);
    }

    private IncidentNode MaterializeIncidentNode(Random random, ScheduleData data, Guid skeletonId)
    {
        var availableIncidentData = data.AvailableIncidentEntities;
        if (availableIncidentData.Count == 0) { throw new InvalidOperationException("[NodeGenerator] availableIncidentData is empty"); }

        var selecetEntity = availableIncidentData[random.Next(availableIncidentData.Count)];

        return new IncidentNode(skeletonId, selecetEntity);
    }
}