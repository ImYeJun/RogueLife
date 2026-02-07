using System;
using System.Collections.Generic;
using System.Linq;

public class ScheduleGenerator
{
    private ScheduleGenerationContext generationContext;
    private ScheduleSkeletonGenerator skeletonGenerator;
    private SchedulePathCollector pathCollector;
    private NodeGenerator nodeGenerator;
    private SchedulePathCountRule pathCountRule;

    public ScheduleGenerator(ScheduleSkeletonRule skeletonRule, SchedulePathRule pathRule, BattleSystem battleSystem, SchedulePathCountRule pathCountRule)
    {
        generationContext = new ScheduleGenerationContext();
        skeletonGenerator = new ScheduleSkeletonGenerator(skeletonRule);
        pathCollector = new SchedulePathCollector(pathRule, generationContext);
        nodeGenerator = new NodeGenerator(battleSystem);

        this.pathCountRule = pathCountRule;
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
        
        UnityEngine.Debug.Log($"attempt count : {attempts}");
        return result;
    }

    private bool TryGenerateSchedule(Random random, ScheduleData scheduleData, out Schedule schedule)
    {
        generationContext.ResetContext();

        ScheduleSkeleton scheduleSkeleton = skeletonGenerator.GenerateSkeleton(random);
        
        pathCollector.StartCollect(scheduleSkeleton.StartNode);

        ResolveSkeletonNodeType(random);

        if (IsAppropriatePathCount())
        {
            schedule = MaterializeSchedule(scheduleSkeleton);
            return true;
        }
        else
        {
            schedule = null;
            return false;
        }
    }

    private bool IsAppropriatePathCount()
    {
        return 
            generationContext.CompletePathCount >= pathCountRule.MinCompeletePath &&
            generationContext.CompletePathCount <= pathCountRule.MaxCompletePath;
    }

    private void ResolveSkeletonNodeType(Random random)
    {
        var completedPath = generationContext.CompletePaths;
        var passingByPathsOnNode = generationContext.PassingByPathsOnNode;

        UnityEngine.Debug.Log($"total complte paths count : {completedPath.Count}");
        var removeTargets = new HashSet<SchedulePath>();
        foreach (var pair in passingByPathsOnNode)
        {
            var node = pair.Key;
            var typeCount = new Dictionary<NodeType, int>();

            foreach (var path in pair.Value)
            {
                NodeType type = path.VisitedNodes[node];
                typeCount.TryAdd(type, 0);
                typeCount[type]++;
            }

            bool isAllSameCount = typeCount.Values.All(value => value == typeCount.Values.First());
            NodeType fixedType;

            if (isAllSameCount) { 
                var keyList = typeCount.Keys.ToList();
                fixedType = keyList[random.Next(keyList.Count)];
            }
            else { fixedType = typeCount.OrderByDescending(key => key.Value).First().Key;} 

            node.FixedType = fixedType;

            foreach (var path in pair.Value)
            {
                if (path.VisitedNodes[node] != fixedType)
                {
                    removeTargets.Add(path);
                }
            }
        }

        foreach (var path in removeTargets)
        {
            generationContext.CompletePaths.Remove(path);
        }
    }

    private Schedule MaterializeSchedule(ScheduleSkeleton skeleton)
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
                if (!layered.ContainsKey(layer + 1)) { throw new InvalidOperationException("The layer is not contiguous"); }
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
                            nextNode.LinkPreviousNode(node); //! TODO Refactor this shit hack.
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