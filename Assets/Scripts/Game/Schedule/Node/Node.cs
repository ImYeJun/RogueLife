using System;
using System.Collections.Generic;

public abstract class Node
{
    protected FieldContext context;
    protected IScheduleRouter nodeFlowHandler;
    protected ScheduleHistory scheduleHistory;

    Guid skeletonId;
    protected List<Node> previousNodes = new List<Node>();
    protected List<Node> nextNodes = new List<Node>();
    private Node exitNode;

    public Guid SkeletonId { get => skeletonId; }
    public IReadOnlyList<Node> PreviousNodes { get => previousNodes; }
    public IReadOnlyList<Node> NextNodes { get => nextNodes; }
    public Node ExitNode { get => exitNode; }
    public ScheduleHistory ScheduleHistory { get => scheduleHistory;  }

    public void LinkNextNode(Node nextNode) { nextNodes.Add(nextNode); }
    public void LinkPreviousNode(Node previousNode) { previousNodes.Add(previousNode); }
    public void FixExitNode(Node exitNode) { this.exitNode = exitNode; }


    public Node(Guid skeletonId)
    {
        this.skeletonId = skeletonId;
    }

    public virtual void OnEnter(FieldContext context, IScheduleRouter nodeFlowHandler, ScheduleHistory scheduleHistory)
    {
        this.context = context;
        this.nodeFlowHandler = nodeFlowHandler;
        this.scheduleHistory = scheduleHistory;
    }

    protected void RecordBelongingsEquipping()
    {
        var mainBelongingsBag = context.BelongingsBag.MainBelongingsBag;

        foreach (var pair in mainBelongingsBag)
        {
            scheduleHistory.RecordEquippedBelongings(pair.Key);
        }
    }

    public void RequestNextNodeSelection()
    {
        nodeFlowHandler.RequestNextNodeSelection(nextNodes);
    }

    public virtual void OnExit(Node nextNode)
    {
        nodeFlowHandler.MoveNode(nextNode, context, nodeFlowHandler, scheduleHistory);

        context = null;
        nodeFlowHandler = null;
        scheduleHistory = null;
    }

    public void OnPlayerMentalBroken()
    {
        //TODO : 멘탈 붕괴 연출
        
        scheduleHistory.HasMentalBroken = true;
        OnExit(exitNode);
    }

    public void OnEarlyExit()
    {
        //TODO : 중간에 나가기 (세이브 기능 구현하기)

        scheduleHistory.HasEarlyExited = true;
        OnExit(exitNode);
    }
}