using System.Collections.Generic;
using System.Linq;

public class ScheduleSkeleton
{
    private Dictionary<int, HashSet<NodeSkeleton>> layeredNodes = new Dictionary<int, HashSet<NodeSkeleton>>(); // <layer, Node>
    private NodeSkeleton startNode;
    private readonly int preEarlyLayerCount;
    private readonly int earlyLayerCount;
    private readonly int middleLayerCount;
    private readonly int lateLayerCount;
    private readonly int postLayerLayerCount;

    public ScheduleSkeleton(NodeSkeleton startNode, int preEarlyLayerCount, int earlyLayerCount, int middleLayerCount, int lateLayerCount, int postLayerLayerCount)
    {
        this.preEarlyLayerCount = preEarlyLayerCount;
        this.earlyLayerCount = earlyLayerCount;
        this.middleLayerCount = middleLayerCount;
        this.lateLayerCount = lateLayerCount;
        this.postLayerLayerCount = postLayerLayerCount;

        this.startNode = startNode;
        AddNode(startNode);
    }

    public Dictionary<int, HashSet<NodeSkeleton>> LayeredNodes { get => layeredNodes; }
    public int FirstLayer { get => layeredNodes.Keys.OrderBy(l => l).First(); }
    public int LastLayer { get => layeredNodes.Keys.OrderByDescending(l => l).First(); }
    public Dictionary<int, HashSet<NodeSkeleton>> EarlyLayers
    {
        get
        {
            // if (!IsValid()) { return null; }

            Dictionary<int, HashSet<NodeSkeleton>> layers = new Dictionary<int, HashSet<NodeSkeleton>>();

            int fisrtLayer = FirstLayer + preEarlyLayerCount;
            for (int i = 0; i < earlyLayerCount; i++)
            {
                int layer = fisrtLayer + i;
                layers.Add(layer, layeredNodes[layer]);
            }

            return layers;
        }
    }
    public Dictionary<int, HashSet<NodeSkeleton>> MiddleLayers
    {
        get
        {
            // if (!IsValid()) { return null; }

            Dictionary<int, HashSet<NodeSkeleton>> layers = new Dictionary<int, HashSet<NodeSkeleton>>();

            int fisrtLayer = FirstLayer + preEarlyLayerCount + earlyLayerCount;
            for (int i = 0; i < middleLayerCount; i++)
            {
                int layer = fisrtLayer + i;
                layers.Add(layer, layeredNodes[layer]);
            }

            return layers;
        }
    }
    public Dictionary<int, HashSet<NodeSkeleton>> LateLayers
    {
        get
        {
            // if (!IsValid()) { return null; }
            
            Dictionary<int, HashSet<NodeSkeleton>> layers = new Dictionary<int, HashSet<NodeSkeleton>>();

            int fisrtLayer = FirstLayer + preEarlyLayerCount + earlyLayerCount + middleLayerCount;
            for (int i = 0; i < lateLayerCount; i++)
            {
                int layer = fisrtLayer + i;
                layers.Add(layer, layeredNodes[layer]);
            }

            return layers;
        }
    }

    public NodeSkeleton StartNode { get => startNode;  }
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

    public void AddNode(NodeSkeleton node)
    {
        if (!layeredNodes.ContainsKey(node.Layer))
        {
            layeredNodes.Add(node.Layer, new HashSet<NodeSkeleton>());
        }
        
        layeredNodes[node.Layer].Add(node);
    }

    public bool IsValid()
    {
        var layers = layeredNodes.Keys.OrderBy(l => l).ToList();

        if (layers == null || layers.Count == 0) { return false; }

        for (int i = 0; i < layers.Count - 2; i++)
        {
            if (layers[i] != layers[i + 1] - 1)
            {
                return false;
            }
        }

        return layers.Count == (earlyLayerCount + middleLayerCount + lateLayerCount);
    }
}