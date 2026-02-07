using System.Collections.Generic;

public class SchedulePathRule
{
    private int maxBattleSequence = 0;
    private int maxIncidentSequence = 0;
    private int maxTransactionSequence = 0;
    private int minBattleCount = 0;
    private int maxBattleCount = 0;
    private int minIncidentCount = 0;
    private int maxIncidentCount = 0;
    private int minTransactionCount = 0;
    private int maxTransactionCount = 0;

    public SchedulePathRule(int maxBattleSequence, int maxIncidentSequence, int maxTransactionSequence, int minBattleCount, int maxBattleCount, int minIncidentCount, int maxIncidentCount, int minTransactionCount, int maxTransactionCount)
    {
        this.maxBattleSequence = maxBattleSequence;
        this.maxIncidentSequence = maxIncidentSequence;
        this.maxTransactionSequence = maxTransactionSequence;
        this.minBattleCount = minBattleCount;
        this.maxBattleCount = maxBattleCount;
        this.minIncidentCount = minIncidentCount;
        this.maxIncidentCount = maxIncidentCount;
        this.minTransactionCount = minTransactionCount;
        this.maxTransactionCount = maxTransactionCount;
    }

    public int MaxBattleSequence { get => maxBattleSequence; }
    public int MaxIncidentSequence { get => maxIncidentSequence; }
    public int MaxTransactionSequence { get => maxTransactionSequence; }
    public int MinBattleCount { get => minBattleCount; }
    public int MaxBattleCount { get => maxBattleCount; }
    public int MinIncidentCount { get => minIncidentCount; }
    public int MaxIncidentCount { get => maxIncidentCount; }
    public int MinTransactionCount { get => minTransactionCount; }
    public int MaxTransactionCount { get => maxTransactionCount; }

    public List<NodeType> GetAvailableType(SchedulePath path)
    {
        var result = new List<NodeType>();

        if (path.RecentBattleSequence <= maxBattleSequence) { result.Add(NodeType.BATTLE); }
        if (path.RecentBattleSequence <= maxIncidentSequence) { result.Add(NodeType.INCIDENT); }
        if (path.RecentBattleSequence <= maxTransactionSequence) { result.Add(NodeType.TRANSACION); }

        return result;
    }

    public bool IsFinalValid(SchedulePath path)
    {
        return 
            path.TotalBattleCount >= minBattleCount && path.TotalBattleCount <= maxBattleCount &&
            path.TotalIndcidentCount >= minIncidentCount && path.TotalIndcidentCount <= maxIncidentCount &&
            path.TotalTransactionCount >= minTransactionCount && path.TotalTransactionCount <= maxTransactionCount;
    }
}