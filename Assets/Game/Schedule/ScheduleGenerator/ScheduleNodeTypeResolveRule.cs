using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScheduleNodeTypeResolveRule
{
    private int maxBattleSequence = 0;
    private int maxIncidentSequence = 0;
    private int maxTransactionSequence = 0;
    private ScheduleLayerZoneRule earlyLayerZoneRule;
    private ScheduleLayerZoneRule middleLayerZoneRule;
    private ScheduleLayerZoneRule lateLayerZoneRule;

    public ScheduleNodeTypeResolveRule(int maxBattleSequence, int maxIncidentSequence, int maxTransactionSequence, ScheduleLayerZoneRule earlyLayerZoneRule, ScheduleLayerZoneRule middleLayerZoneRule, ScheduleLayerZoneRule lateLayerZoneRule)
    {
        this.maxBattleSequence = maxBattleSequence;
        this.maxIncidentSequence = maxIncidentSequence;
        this.maxTransactionSequence = maxTransactionSequence;
        this.earlyLayerZoneRule = earlyLayerZoneRule;
        this.middleLayerZoneRule = middleLayerZoneRule;
        this.lateLayerZoneRule = lateLayerZoneRule;
    }

    public int MaxBattleSequence { get => maxBattleSequence; }
    public int MaxIncidentSequence { get => maxIncidentSequence; }
    public int MaxTransactionSequence { get => maxTransactionSequence; }
    public ScheduleLayerZoneRule EarlyLayerZoneRule { get => earlyLayerZoneRule; }
    public ScheduleLayerZoneRule MiddleLayerZoneRule { get => middleLayerZoneRule; }
    public ScheduleLayerZoneRule LateLayerZoneRule { get => lateLayerZoneRule; }

    public NodeType RequestResolveNodeType(System.Random random, ScheduleLayerZoneType type, Dictionary<NodeType, int> accumulateState)
    {
        ScheduleLayerZoneRule rule = SelectRule(type);

        //* Weighted Random Algorithm
        int totalWeight = rule.BattleNodeWeight + rule.IncidentNodeWeight + rule.TransactionNodeWeight;

        if (totalWeight <= 0) { throw new InvalidOperationException("Total weight cannot be negative."); }

        double battleNodeWeight = (double)rule.BattleNodeWeight / totalWeight;
        battleNodeWeight = Math.Pow(battleNodeWeight, accumulateState[NodeType.BATTLE] + 1);

        double incidentNodeWeight = (double)rule.IncidentNodeWeight / totalWeight;
        incidentNodeWeight = Math.Pow(incidentNodeWeight, accumulateState[NodeType.INCIDENT] + 1);

        double transactionNodeWeight = (double)rule.TransactionNodeWeight / totalWeight;
        transactionNodeWeight = Math.Pow(transactionNodeWeight, accumulateState[NodeType.TRANSACTION] + 1);

        double convertedTotalWeight = battleNodeWeight + incidentNodeWeight + transactionNodeWeight;
        
        List<KeyValuePair<NodeType, double>> pool = new()
        {
            new(NodeType.BATTLE, battleNodeWeight),
            new(NodeType.INCIDENT, incidentNodeWeight),
            new(NodeType.TRANSACTION, transactionNodeWeight)
        };
        pool = pool.OrderBy(e => e.Value).ToList();

        double pivot = random.NextDouble() * convertedTotalWeight;
        double currentWeight = 0;

        foreach (var pair in pool)
        {
            currentWeight += pair.Value;

            if (currentWeight >= pivot) { return pair.Key; }
        }

        var lastItem = pool[pool.Count - 1];
        return lastItem.Key;
    }

    public bool IsNodeTypeCountValid(ScheduleLayerZoneType type, int totalBattleNodeCount, int totalIncidentNodeCount, int totalTransactionNodeCount)
    {
        ScheduleLayerZoneRule rule = SelectRule(type);

        return 
            totalBattleNodeCount >= rule.MinBattleNodeCount &&
            totalIncidentNodeCount >= rule.MinIncidentNodeCount &&
            totalTransactionNodeCount >= rule.MinTransactionNodeCount;
    }

    private ScheduleLayerZoneRule SelectRule(ScheduleLayerZoneType type)
    {
        return type switch
        {
            ScheduleLayerZoneType.EARLY => earlyLayerZoneRule,
            ScheduleLayerZoneType.MIDDLE => middleLayerZoneRule,
            ScheduleLayerZoneType.LATE => lateLayerZoneRule,
            _ => throw new InvalidOperationException($"{type} is not supproted for resolving node type.")
        };
    }
}