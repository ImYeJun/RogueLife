using System;
using System.Collections.Generic;

public class Schedule : IChoiceScheduleSystem
{
    private ScheduleData scheduleData;
    private Node startNode;
    private Node currentNode;
    private EnemyDataSlot bossDataSlot;
    private Dictionary<int, List<Node>> map;

    public Dictionary<int, List<Node>> Map { get => map; }
    public void FixMap(Dictionary<int, List<Node>> map) { this.map = map; }
    public void FixStartNode(Node startNode) { this.startNode = startNode; }

    public event Action OnEnd; 

    public void MoveNode(Node nextNode)
    {
        
    }

    public void SetBoss(EnemyData bossData)
    {
        bossDataSlot.Data = bossData;
    }

    public void EndSchedule()
    {
        OnEnd?.Invoke();
    }
}