using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class ScheduleNodeTypeResolver
{
    private ScheduleNodeTypeResolveRule rule;
    private System.Random random;
    private ScheduleSkeleton skeleton;
    private Dictionary<NodeType, int> currentLayerNodeTypeAccumulateState = new Dictionary<NodeType, int>();
    private void RefreshTypeAccumulateState()
    {
        currentLayerNodeTypeAccumulateState.Clear();
        currentLayerNodeTypeAccumulateState.Add(NodeType.BATTLE, 0);
        currentLayerNodeTypeAccumulateState.Add(NodeType.INCIDENT, 0);
        currentLayerNodeTypeAccumulateState.Add(NodeType.TRANSACTION, 0);
    }

    public ScheduleNodeTypeResolver(ScheduleNodeTypeResolveRule rule)
    {
        this.rule = rule;
    }

    public void ResolveSkeletonNodeType(System.Random random, ScheduleSkeleton skeleton)
    {
        this.random = random;
        this.skeleton = skeleton;

        int attempts = 0;
        while (true)
        {
            if (TryResolveSkeletonNodeType()) { break; }
            if (++attempts >= Constant.MAX_SCHEDULE_NODE_RESOLVE_ATTEMPTS) { throw new InvalidOperationException("Maximum attempts exceeded while resolving schedule skeleton node type."); }
        }
    }

    private bool TryResolveSkeletonNodeType()
    {
        int attempts;

        foreach (var node in skeleton.Nodes) { node.InitializeAscenedentSequenceState(); }

        attempts = 0;
        while (true)
        {
            if (TryResolveLayerZone(ScheduleLayerZoneType.EARLY)) { break; }
            if (++attempts >= Constant.MAX_SCHEDULE_NODE_RESOLVE_ATTEMPTS) { throw new InvalidOperationException("Maximum attempts exceeded while resolving schedule skeleton node type in early layer zone."); }
        }

        attempts = 0;
        while (true)
        {
            if (TryResolveLayerZone(ScheduleLayerZoneType.MIDDLE)) { break; }
            if (++attempts >= Constant.MAX_SCHEDULE_NODE_RESOLVE_ATTEMPTS) { throw new InvalidOperationException("Exceeded schedule skeleton node type resolve attempts in middle layer zone."); }
        }

        attempts = 0;
        while (true)
        {
            if (TryResolveLayerZone(ScheduleLayerZoneType.LATE)) { break; }
            if (++attempts >= Constant.MAX_SCHEDULE_NODE_RESOLVE_ATTEMPTS) { throw new InvalidOperationException("Exceeded schedule skeleton node type resolve attempts in late layer zone"); }
        }

        return true;
    }

    private bool TryResolveLayerZone(ScheduleLayerZoneType type)
    {
        var layers = type switch
        {
            ScheduleLayerZoneType.EARLY => skeleton.EarlyLayers,
            ScheduleLayerZoneType.MIDDLE => skeleton.MiddleLayers,
            ScheduleLayerZoneType.LATE => skeleton.LateLayers,
            _ => throw new InvalidOperationException($"{type} is not supproted for selecting layer zone.")
        };

        int totalBattleNodeCount = 0;
        int totalIncidentNodeCount = 0;
        int totalTransactionNodeCount = 0;

        foreach (var layer in layers)
        {
            RefreshTypeAccumulateState();
            
            foreach (var node in layer.Value)
            {
                if (node.FixedType == NodeType.ENTRY || node.FixedType == NodeType.EXIT) { continue; }
                node.ResetSequenceStateFromPreviousLayer();
                NodeType resolvedType = rule.RequestResolveNodeType(random, type, currentLayerNodeTypeAccumulateState);

                switch (resolvedType)
                {
                    case NodeType.BATTLE:
                        totalBattleNodeCount++;
                        node.SpawnEnemyTier = rule.RequestResolveEnemyTier(random, type);
                        break;
                    case NodeType.INCIDENT:
                        totalIncidentNodeCount++;
                        break;
                    case NodeType.TRANSACTION:
                        totalTransactionNodeCount++;
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid node type for resolved : {resolvedType}");
                }

                if (!node.IsAscendentsSequenceValid(rule)) { return false; }

                currentLayerNodeTypeAccumulateState[resolvedType]++;
                node.FixedType = resolvedType;
            }
        }

        return rule.IsNodeTypeCountValid(type, totalBattleNodeCount, totalIncidentNodeCount, totalTransactionNodeCount);
    }
}