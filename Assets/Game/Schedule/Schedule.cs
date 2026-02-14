using System;
using System.Collections.Generic;
using System.Linq;

public class Schedule : IFieldScheduleSystem
{
    private ScheduleHistory history = new ScheduleHistory();
    private ScheduleData data;
    private Node startNode;
    private Node exitNode;
    private Node currentNode;
    private EnemyDataSlot bossDataSlot;
    private Dictionary<int, List<Node>> map;

    public Dictionary<int, List<Node>> Map { get => map; }
    
    public void FixData(ScheduleData data) { this.data = data; }
    public void FixMap(Dictionary<int, List<Node>> map) { this.map = map; }
    public void FixStartNode(Node startNode) { this.startNode = startNode; }
    public void SetBossDataSlot(EnemyDataSlot slot) { this.bossDataSlot = slot; }
    public void FixExitNode(Node exitNode)
    {
        this.exitNode = exitNode;
        foreach (var layer in map.Values)
        {
            foreach (var node in layer)
            {
                node.FixExitNode(exitNode);
            }
        }
    }

    public event Action<ScheduleHistory> OnEnd; 

    public void EnterStartNode(Player player, FieldContext context) { 
        currentNode = null;
        MoveNode(startNode, player, context);
    }
    public void MoveNode(Node nextNode, Player player, FieldContext context)
    {
        if (currentNode != null && !currentNode.NextNodes.Contains(nextNode) && nextNode != exitNode) 
        { 
            throw new InvalidOperationException("The given node is not connected to the current node."); 
        }

        currentNode = nextNode;
        nextNode.OnEnter(player, context, history);
    }

    public void SetBossData(EnemyData bossData)
    {
        bossDataSlot.Data = bossData;
    }

    public void EndSchedule()
    {
        OnEnd?.Invoke(history);
    }
}