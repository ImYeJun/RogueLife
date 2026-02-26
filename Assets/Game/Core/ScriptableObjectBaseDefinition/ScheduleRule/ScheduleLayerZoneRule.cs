using UnityEngine;


[CreateAssetMenu(fileName = "ScheduleLayerZoneRule", menuName = "Scriptable Objects/ScheudleRule/ScheduleLayerZoneRule")]
public class ScheduleLayerZoneRule : ScriptableObject
{
    [SerializeField, Min(0)] private int battleNodeWeight;
    [SerializeField, Min(0)] private int incidentNodeWeight;
    [SerializeField, Min(0)] private int transactionNodeWeight;
    [SerializeField, Min(0)] private int minBattleNodeCount;
    [SerializeField, Min(0)] private int minIncidentNodeCount;
    [SerializeField, Min(0)] private int minTransactionNodeCount;
    [SerializeField, Min(0)] private int normalEnemySpawnWeight;
    [SerializeField, Min(0)] private int eliteEnemySpawnWeight;

    public ScheduleLayerZoneRule(int battleNodeWeight, int incidentNodeWeight, int transactionNodeWeight, int minBattleNodeCount, int minIncidentNodeCount, int minTransactionNodeCount, int normalEnemySpawnWeight, int eliteEnemySpawnWeight)
    {
        this.battleNodeWeight = battleNodeWeight;
        this.incidentNodeWeight = incidentNodeWeight;
        this.transactionNodeWeight = transactionNodeWeight;
        this.minBattleNodeCount = minBattleNodeCount;
        this.minIncidentNodeCount = minIncidentNodeCount;
        this.minTransactionNodeCount = minTransactionNodeCount;
        this.normalEnemySpawnWeight = normalEnemySpawnWeight;
        this.eliteEnemySpawnWeight = eliteEnemySpawnWeight;
    }

    public int BattleNodeWeight { get => battleNodeWeight; }
    public int IncidentNodeWeight { get => incidentNodeWeight; }
    public int TransactionNodeWeight { get => transactionNodeWeight; }
    public int MinBattleNodeCount { get => minBattleNodeCount; }
    public int MinIncidentNodeCount { get => minIncidentNodeCount; }
    public int MinTransactionNodeCount { get => minTransactionNodeCount; }
    public int NormalEnemySpawnWeight { get => normalEnemySpawnWeight; }
    public int EliteEnemySpawnWeight { get => eliteEnemySpawnWeight; }
}