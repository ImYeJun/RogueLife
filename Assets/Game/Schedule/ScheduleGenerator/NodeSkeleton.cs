using System;
using System.Collections.Generic;

public class NodeSkeleton
{
    private Guid id;
    private int layer = 0;
    private NodeType fixedType = NodeType.NONE;
    private List<NodeSkeleton> previousNodes = new List<NodeSkeleton>();
    private List<NodeSkeleton> nextNodes = new List<NodeSkeleton>();

    public NodeSkeleton(int layer, Guid id)
    {
        this.layer = layer;
    }

    public NodeSkeleton(int layer, Guid id, NodeType fixedType) : this(layer, id)
    {
        this.fixedType = fixedType;
    }

    public Guid Id { get => id;  }
    public int Layer { get => layer; }
    public NodeType FixedType { get => fixedType; set => fixedType = value; }
    public List<NodeSkeleton> PreviousNodes { get => previousNodes; }
    public List<NodeSkeleton> NextNodes { get => nextNodes; }
}