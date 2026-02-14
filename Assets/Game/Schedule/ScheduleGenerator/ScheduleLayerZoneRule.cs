using UnityEngine;

public class ScheduleLayerZoneRule
{
    [Min(0)] private int battleNodeWeight;
    [Min(0)] private int incidentNodeWeight;
    [Min(0)] private int transactionNodeWeight;
    [Min(0)] private int minBattleNodeCount;
    [Min(0)] private int minIncidentNodeCount;
    [Min(0)] private int minTransactionNodeCount;
    
    public ScheduleLayerZoneRule(
        int battleNodeWeight,
        int incidentNodeWeight,
        int transactionNodeWeight,
        int minBattleNodeCount,
        int minIncidentNodeCount,
        int minTransactionNodeCount
    )
    {
        this.battleNodeWeight = battleNodeWeight;
        this.incidentNodeWeight = incidentNodeWeight;
        this.transactionNodeWeight = transactionNodeWeight;
        this.minBattleNodeCount = minBattleNodeCount;
        this.minIncidentNodeCount = minIncidentNodeCount;
        this.minTransactionNodeCount = minTransactionNodeCount;
    }

    public int BattleNodeWeight { get => battleNodeWeight; }
    public int IncidentNodeWeight { get => incidentNodeWeight; }
    public int TransactionNodeWeight { get => transactionNodeWeight; }
    public int MinBattleNodeCount { get => minBattleNodeCount; }
    public int MinIncidentNodeCount { get => minIncidentNodeCount; }
    public int MinTransactionNodeCount { get => minTransactionNodeCount; }
}