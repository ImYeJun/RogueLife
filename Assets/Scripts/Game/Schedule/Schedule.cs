using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ViewEvent.ScheduleView;

public class Schedule : IReadOnlySchedule, IScheduleRouter
{
    private ScheduleHistory history = new ScheduleHistory();
    private ScheduleData data;
    private Node startNode;
    private Node exitNode;
    private Node currentNode;
    private EnemyDataSlot bossDataSlot;
    private Dictionary<int, List<Node>> map;

    private bool hasStarted = false;

    public bool HasStarted => hasStarted;
    public ScheduleData Data { get => data; }
    public IReadOnlyDictionary<int, List<Node>> Map => map;

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
    public event Action<Node> OnNodeEnter;
    public event Action<Node> OnNodeExit;
    public event Action<List<Node>> OnRequestNextNodeSelection;
    public event Action<Dictionary<TransactionChoiceOrder, TransactionChoiceData>> OnRequestTransactionSelection;
    public event Action<List<DeterminedIncidentChoice>> OnRequestIncidentSelection;
    public event Action OnRequestBattleTransition;


    public void EnterStartNode(FieldContext context) { 
        currentNode = null;
        hasStarted = true;
        MoveNode(startNode, context, this, history);
    }
    public void MoveNode(Node nextNode, FieldContext context, IScheduleRouter nodeFlowHandler, ScheduleHistory scheduleHistory)
    {
        if (currentNode != null && !currentNode.NextNodes.Contains(nextNode) && nextNode != exitNode) 
        { 
            throw new InvalidOperationException("The given node is not connected to the current node."); 
        }

        currentNode = nextNode;
        
        OnNodeEnter?.Invoke(currentNode);
        nextNode.OnEnter(context, nodeFlowHandler, scheduleHistory);
    }

    public void SetBossData(EnemyEntity bossEntity)
    {
        bossDataSlot.Entity = bossEntity;
    }

    public void EndSchedule()
    {
        OnEnd?.Invoke(history);
    }

    public void RequestNextNodeSelection(List<Node> nextNodes)
    {
        OnRequestNextNodeSelection?.Invoke(nextNodes);
    }
    public void RequestTransactionSelection(Dictionary<TransactionChoiceOrder, TransactionChoiceData> choices)
    {
        OnRequestTransactionSelection?.Invoke(choices);
    }
    public void RequestIncidentSelection(List<DeterminedIncidentChoice> choices)
    {
        OnRequestIncidentSelection?.Invoke(choices);
    }
    public void RequestBattleTransition()
    {
        OnRequestBattleTransition?.Invoke();
    }

    public void SettleNextNode(Node nextNode)
    {
        if (currentNode is null) { return; }

        OnNodeExit?.Invoke(currentNode);
        currentNode.OnExit(nextNode);
    }

    public void SettleTransactionChoice(TransactionChoiceOrder order)
    {
        if (currentNode is not TransactionNode transactionNode)
        {
            UnityEngine.Debug.LogWarning($"[Schedule] Current node is not a TransactionNode, but a transaction choice was settled.");

            return;
        }

        transactionNode.OnChoiceSettled(order);
    }

    public void SettleIncidentChoice(DeterminedIncidentChoice choice)
    {
        if (currentNode is not IncidentNode incidentNode)
        {
            UnityEngine.Debug.LogWarning($"[Schedule] Current node is not a IncidentNode, but a incident choice was settled.");

            return;
        }

        incidentNode.OnChoiceSettled(choice);
    }
}