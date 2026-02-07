using System.Collections.Generic;

public class SchedulePath
{
    private int recentBattleSequence = 0;
    private int recentIndicentSequence = 0;
    private int recentTransactionSeqeuence = 0;
    private int totalBattleCount = 0;
    private int totalIndcidentCount = 0;
    private int totalTransactionCount = 0;
    private Dictionary<NodeSkeleton, NodeType> visitedNodes = new Dictionary<NodeSkeleton, NodeType>();

    public int RecentBattleSequence { get => recentBattleSequence; }
    public int RecentIndicentSequence { get => recentIndicentSequence; }
    public int RecentTransactionSeqeuence { get => recentTransactionSeqeuence; }
    public int TotalBattleCount { get => totalBattleCount; }
    public int TotalIndcidentCount { get => totalIndcidentCount; }
    public int TotalTransactionCount { get => totalTransactionCount; }
    public Dictionary<NodeSkeleton, NodeType> VisitedNodes { get => visitedNodes; }

    
    public SchedulePath Clone()
    {
        return new SchedulePath
        {
            recentBattleSequence = this.recentBattleSequence,
            recentIndicentSequence = this.recentIndicentSequence,
            recentTransactionSeqeuence = this.recentTransactionSeqeuence,
            totalBattleCount = this.totalBattleCount,
            totalIndcidentCount = this.totalIndcidentCount,
            totalTransactionCount = this.totalTransactionCount,
            visitedNodes = new Dictionary<NodeSkeleton, NodeType>(this.visitedNodes)
        };
    }

    public void ApplyNode(NodeSkeleton node, NodeType type)
    {
        visitedNodes[node] = type;

        recentBattleSequence = type == NodeType.BATTLE ? recentBattleSequence + 1 : 0;
        recentIndicentSequence = type == NodeType.INCIDENT ? recentIndicentSequence + 1 : 0;
        recentTransactionSeqeuence = type == NodeType.TRANSACION ? recentTransactionSeqeuence + 1 : 0;

        if (type == NodeType.BATTLE) { totalBattleCount++; }
        if (type == NodeType.INCIDENT) { totalIndcidentCount++; }
        if (type == NodeType.TRANSACION) { totalTransactionCount++; }
    }

    public bool Contains(NodeSkeleton node)
    {
        return visitedNodes.ContainsKey(node);
    }
}