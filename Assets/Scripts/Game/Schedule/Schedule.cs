using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ViewEvent.ScheduleView;

public class Schedule
{
    private ScheduleHistory history = new ScheduleHistory();
    private ScheduleData data;
    private Node startNode;
    private Node exitNode;
    private Node currentNode;
    private EnemyDataSlot bossDataSlot;
    private Dictionary<int, List<Node>> map;

    private bool hasStarted = false;

    public Dictionary<int, List<Node>> Map { get => map; }

    public bool HasStarted => hasStarted;

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
    public event Action<Node> OnNodeMoved;

    public void EnterStartNode(FieldContext context) { 
        currentNode = null;
        hasStarted = true;
        MoveNode(startNode, context);
    }
    public void MoveNode(Node nextNode, FieldContext context)
    {
        if (currentNode != null && !currentNode.NextNodes.Contains(nextNode) && nextNode != exitNode) 
        { 
            throw new InvalidOperationException("The given node is not connected to the current node."); 
        }

        currentNode = nextNode;
        
        OnNodeMoved?.Invoke(currentNode);
        nextNode.OnEnter(context, history);
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