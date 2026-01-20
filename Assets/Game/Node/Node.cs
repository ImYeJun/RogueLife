using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private List<Node> previousNodes;
    private List<Node> nexNodes;
    public event Action<Node> OnMoveRequest;

    public abstract void OnEnter();
    public abstract void OnExit();
}