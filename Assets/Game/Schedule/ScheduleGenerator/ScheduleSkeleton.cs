using System.Collections.Generic;

public class ScheduleSkeleton
{
    private Dictionary<int, HashSet<NodeSkeleton>> layeredNodes = new Dictionary<int, HashSet<NodeSkeleton>>(); // <layer, Node>
    private NodeSkeleton startNode;

    public ScheduleSkeleton(NodeSkeleton startNode)
    {
        this.startNode = startNode;

        AddNode(startNode);
    }

    public Dictionary<int, HashSet<NodeSkeleton>> LayeredNodes { get => layeredNodes; }
    public List<NodeSkeleton> Nodes 
    { 
        get
        {
            List<NodeSkeleton> nodes = new List<NodeSkeleton>();

            foreach (var pair in layeredNodes)
            {
                foreach (var element in pair.Value)
                {
                    nodes.Add(element);
                }
            }

            return nodes;
        }
    }
    public NodeSkeleton StartNode { get => startNode;  }

    public void AddNode(NodeSkeleton node)
    {
        if (!layeredNodes.ContainsKey(node.Layer))
        {
            layeredNodes.Add(node.Layer, new HashSet<NodeSkeleton>());
        }
        
        layeredNodes[node.Layer].Add(node);
    }
}