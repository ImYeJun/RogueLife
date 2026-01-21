using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private List<Node> previousNodes = new List<Node>();
    private List<Node> nexNodes = new List<Node>();
    public event Action<Node> OnMoveRequest;

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