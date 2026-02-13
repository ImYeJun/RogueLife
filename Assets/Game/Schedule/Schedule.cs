using System;
using System.Collections.Generic;
using System.Linq;

public class Schedule : IFieldScheduleSystem
{
    private ScheduleData data;
    private Node startNode;
    private Node currentNode;
    private EnemyDataSlot bossDataSlot;
    private Dictionary<int, List<Node>> map;

    public Dictionary<int, List<Node>> Map { get => map; }
    
    public void FixData(ScheduleData data) { this.data = data; }
    public void FixMap(Dictionary<int, List<Node>> map) { this.map = map; }
    public void FixStartNode(Node startNode) { this.startNode = startNode; }
    public void SetBossDataSlot(EnemyDataSlot slot) { this.bossDataSlot = slot; }

    public event Action OnEnd; 

    public void EnterStartNode(Player player) { 
        currentNode = null;
        MoveNode(startNode, player);
    }
    public void MoveNode(Node nextNode, Player player)
    {
        if (currentNode != null && !currentNode.NextNodes.Contains(nextNode)) 
        { 
            throw new InvalidOperationException("The given node is not connected to the current node."); 
        }

        currentNode = nextNode;
        nextNode.OnEnter(player);
    }

    public void SetBossData(EnemyData bossData)
    {
        bossDataSlot.Data = bossData;
    }

    public void EndSchedule()
    {
        OnEnd?.Invoke();
    }
}