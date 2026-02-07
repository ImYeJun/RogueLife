using System.Collections.Generic;

public class NodeSkeleton
{
    private int layer = 0;
    private NodeType fixedType = NodeType.NONE;
    private List<NodeSkeleton> previousNodes = new List<NodeSkeleton>();
    private List<NodeSkeleton> nextNodes = new List<NodeSkeleton>();

    public NodeSkeleton(int layer)
    {
        this.layer = layer;
    }

    public NodeSkeleton(int layer, NodeType fixedType) : this(layer)
    {
        this.fixedType = fixedType;
    }

    public int Layer { get => layer; }
    public NodeType FixedType { get => fixedType; }
    public List<NodeSkeleton> PreviousNodes { get => previousNodes; }
    public List<NodeSkeleton> NextNodes { get => nextNodes; }
}