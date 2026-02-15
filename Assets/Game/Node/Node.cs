using System;
using System.Collections.Generic;

public abstract class Node
{
    protected FieldContext context;
    protected ScheduleHistory scheduleHistory;

    Guid skeletonId;
    private List<Node> previousNodes = new List<Node>();
    private List<Node> nextNodes = new List<Node>();
    private Node exitNode;

    public Guid SkeletonId { get => skeletonId; }
    public IReadOnlyCollection<Node> PreviousNodes { get => previousNodes; }
    public IReadOnlyCollection<Node> NextNodes { get => nextNodes; }
    public Node ExitNode { get => exitNode; }

    public void LinkNextNode(Node nextNode) { nextNodes.Add(nextNode); }
    public void LinkPreviousNode(Node previousNode) { previousNodes.Add(previousNode); }
    public void FixExitNode(Node exitNode) { this.exitNode = exitNode; }

    public Action<Node, FieldContext> OnMoveRequest;

    public Node(Action<Node, FieldContext> OnMoveRequest, Guid skeletonId)
    {
        this.OnMoveRequest = OnMoveRequest;
        this.skeletonId = skeletonId;
    }

    public virtual void OnEnter(FieldContext context, ScheduleHistory scheduleHistory)
    {
        this.context = context;
        this.scheduleHistory = scheduleHistory;
        //TODO : 노드 진입 연출 실행
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
        //TODO : nextNodes에 따라 UI 띄우기 + 선택지 UI에 옵저버로 OnExit() 집어 넣기
    }

    protected virtual void OnExit(Node nextNode)
    {
        //TODO : 노드 퇴장 연출 실행
        OnMoveRequest.Invoke(nextNode, context);
        
        context = null;
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