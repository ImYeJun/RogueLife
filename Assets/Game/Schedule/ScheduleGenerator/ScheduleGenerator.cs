using System;
using System.Collections.Generic;
using System.Linq;

public class ScheduleGenerator
{
    private ScheduleSkeletonGenerator skeletonGenerator;
    private ScheduleNodeTypeResolver nodeTypeResolver;
    private NodeGenerator nodeGenerator;

    public ScheduleGenerator(ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule typeResolveRule, IEngageBattle battleSystem)
    {
        skeletonGenerator = new ScheduleSkeletonGenerator(skeletonRule);
        nodeTypeResolver = new ScheduleNodeTypeResolver(typeResolveRule);
        nodeGenerator = new NodeGenerator(battleSystem);
    }

    public Schedule GenerateSchedule(Random random, ScheduleData scheduleData)
    {
        Schedule result;
        int attempts = 1;
        while (true)
        {
            if (TryGenerateSchedule(random, scheduleData, out result))
            {
                break;
            }
            if (attempts > Constant.MAX_SCHEDULE_GENERATION_ATTEMPTS)
            {
                throw new InvalidOperationException($"Exceed max schedule generation attempts");
            }
            attempts++;
        }
        
        return result;
    }

    private bool TryGenerateSchedule(Random random, ScheduleData scheduleData, out Schedule schedule)
    {
        ScheduleSkeleton scheduleSkeleton = skeletonGenerator.GenerateSkeleton(random);
        
        nodeTypeResolver.ResolveSkeletonNodeType(random, scheduleSkeleton);

        schedule = MaterializeSchedule(scheduleSkeleton, scheduleData);
        return true;
    }

    private Schedule MaterializeSchedule(ScheduleSkeleton skeleton, ScheduleData data)
    {
        Dictionary<int, List<Node>> layered = new Dictionary<int, List<Node>>();
        Schedule schedule = new Schedule();

        foreach (var pair in skeleton.LayeredNodes)
        {
            int layer = pair.Key;
            var layerSkeletonNodes = pair.Value.ToList();

            layered[layer] = new List<Node>();
            foreach (NodeSkeleton nodeSkeleton in layerSkeletonNodes)
            {
                Node node = nodeGenerator.Generate(nodeSkeleton.Id, nodeSkeleton, schedule.MoveNode, schedule.EndSchedule);

                layered[layer].Add(node);
            }
        }

        int maxLayer = layered.Keys.OrderByDescending(l => l).First();
        Node startNode = null;

        foreach (var pair in layered)
        {
            var layer = pair.Key;
            var nodes = pair.Value;

            if (layer != maxLayer)
            {
                if (!layered.ContainsKey(layer + 1)) { throw new InvalidOperationException($"The layer is not contiguous ({layer} -> {layer + 1})"); }
                var nextLayerNodes = layered[layer + 1];

                foreach (var node in nodes)
                {
                    if (node.SkeletonId == skeleton.StartNode.Id) { startNode = node; }

                    NodeSkeleton originNodeSkeleton = skeleton.Nodes.Find(origin => origin.Id == node.SkeletonId);
                    if (originNodeSkeleton == null) { throw new InvalidOperationException("Node Skeleton and Materialized Map is not matched"); }
                    List<Guid> nextSkeltonNodesId = originNodeSkeleton.NextNodes.Select(origin => origin.Id).ToList();

                    foreach (var nextNode in nextLayerNodes)
                    {
                        if (nextSkeltonNodesId.Contains(nextNode.SkeletonId))
                        {
                            node.LinkNextNode(nextNode);
                            nextNode.LinkPreviousNode(node);
                        }
                    }
                }
            }
        }

        if (startNode == null) { throw new InvalidOperationException("Start Node is not found"); }
        schedule.FixMap(layered);
        schedule.FixStartNode(startNode);

        return schedule;
    }
}