using System;
using System.Collections.Generic;

public abstract class Node
{
    Guid skeletonId;
    private List<Node> previousNodes = new List<Node>();
    private List<Node> nextNodes = new List<Node>();

    public Guid SkeletonId { get => skeletonId; }
    public IReadOnlyCollection<Node> PreviousNodes { get => previousNodes; }
    public IReadOnlyCollection<Node> NextNodes { get => nextNodes; }

    public void LinkNextNode(Node nextNode) { nextNodes.Add(nextNode); }
    public void LinkPreviousNode(Node previousNode) { previousNodes.Add(previousNode); }

    public event Action<Node> OnMoveRequest;

    public Node(Action<Node> OnMoveRequest, Guid skeletonId)
    {
        this.OnMoveRequest += OnMoveRequest;
        this.skeletonId = skeletonId;
    }

    public virtual void OnEnter()
    {
        //TODO : 노드 진입 연출 실행
    }
    
    public void RequestNextNodeSelection()
    {
        //TODO : nextNodes에 따라 UI 띄우기
    }

    public void OnExit(Node nextNode)
    {
        //TODO : 노드 퇴장 연출 실행
        OnMoveRequest.Invoke(nextNode);
    }
}