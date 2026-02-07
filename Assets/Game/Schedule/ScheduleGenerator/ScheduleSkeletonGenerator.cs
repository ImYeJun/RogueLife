using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;

public class ScheduleSkeletonGenerator
{
    private ScheduleSkeletonRule rule;
    private System.Random currentRandom;
    private int currentLayer;

    public ScheduleSkeletonGenerator(ScheduleSkeletonRule rule)
    {
        this.rule = rule;
    }

    public ScheduleSkeleton GenerateSkeleton(System.Random random)
    {
        currentRandom = random;
        ScheduleSkeleton skeleton;

        int tryCount = 0;
        while (true)
        {
            if (TryGenerateSkeleton(out skeleton))
            {
                break;
            }

            tryCount++;

            if (tryCount >= Constant.MAX_SCHEDULE_SKELETON_GENERATION_ATTEMPTS)
            {
                throw new Exception($"How is it even possible to fail {tryCount} in Generating Shcedule Skeleton?!!!");
            }
        }

        return skeleton;
    }

    private bool TryGenerateSkeleton(out ScheduleSkeleton skeleton)
    {
        currentLayer = 0;
        NodeSkeleton entryNode = new NodeSkeleton(currentLayer, NodeType.ENTRY);
        ScheduleSkeleton currentSkeleton = new ScheduleSkeleton(entryNode);

        CreateLayers(currentSkeleton);
        if (TryLinkLayers(currentSkeleton))
        {
            NodeSkeleton exitNode = new NodeSkeleton(++currentLayer, NodeType.EXIT);
            currentSkeleton.AddNode(exitNode);

            var exitNodePreviousLayer = currentSkeleton.LayeredNodes[currentLayer - 1];
            foreach (var node in exitNodePreviousLayer)
            {
                node.NextNodes.Add(exitNode);
                exitNode.PreviousNodes.Add(node);
            }

            skeleton = currentSkeleton;
            return true;
        }
        else
        {
            skeleton = null;
            return false;
        }
    }

    private void CreateLayers(ScheduleSkeleton currentSkeleton)
    {
        int layerCount = currentRandom.Next(rule.MinLayer, rule.MaxLayer + 1);

        int previousLayerNodeCount = 1;
        for (int i = 0; i < layerCount; i++)
        {
            currentLayer++;

            int currentLayerNodeCount;
            do
            {
                currentLayerNodeCount = currentRandom.Next(rule.MinNodePerLayer, rule.MaxNodePerLayer);
            }while (currentLayerNodeCount > previousLayerNodeCount * rule.MaxNodeLinkCount);

            for (int j = 0; j < currentLayerNodeCount; j++)
            {
                NodeSkeleton node = new NodeSkeleton(currentLayer);
                currentSkeleton.AddNode(node);
            }

            previousLayerNodeCount = currentLayerNodeCount;
        }
    }

    private bool TryLinkLayers(ScheduleSkeleton currentSkeleton)
    {
        Dictionary<int, HashSet<NodeSkeleton>> layered = currentSkeleton.LayeredNodes;

        var possibleLayers = layered.Keys;

        foreach (int layer in possibleLayers.OrderBy(l => l))
        {
            if (layer == possibleLayers.Min()) { continue; } //* Since the lowest layered doesn't have the previous layer
            if (!possibleLayers.Contains(layer - 1)) { throw new InvalidOperationException("The given ScheduleSkeleton is not layered properly"); }

            List<NodeSkeleton> currentLayer = layered[layer].ToList();
            List<NodeSkeleton> previousLayer = layered[layer - 1].ToList();

            foreach (var currentNode in currentLayer)
            {
                var possiblePreviousTargets = previousLayer.Where(node => node.NextNodes.Count < rule.MaxNodePerLayer).ToList();
                if (possiblePreviousTargets.Count <= 0)
                {
                    Debug.LogError("Fucked Seed in Generating Schedule Skeleton");
                    return false;
                }

                var targetPreviousNode = possiblePreviousTargets[currentRandom.Next(possiblePreviousTargets.Count)];

                currentNode.PreviousNodes.Add(targetPreviousNode);
                targetPreviousNode.NextNodes.Add(currentNode);
            }

            foreach (var previousNode in previousLayer)
            {
                if (previousNode.NextNodes.Count() == 0)
                {
                    var targetCurrentNode = currentLayer[currentRandom.Next(currentLayer.Count)];

                    targetCurrentNode.PreviousNodes.Add(previousNode);
                    previousNode.NextNodes.Add(targetCurrentNode);
                }

                if (previousNode.NextNodes.Count() == 1)
                {
                    int additionalLinkCount = 0;

                    float currentChance = rule.AdditionalLinkMultiplierChance;

                    //Sicne it is ensured that previousNode has at least one next node, the max additional next node must be rule.MaxNodeLinkCount - 1
                    while (currentChance > currentRandom.NextDouble() && additionalLinkCount < rule.MaxNodeLinkCount - 1)
                    {
                        currentChance *= rule.AdditionalLinkMultiplierChance;
                        additionalLinkCount++;
                    }

                    for (int i = 0; i < additionalLinkCount; i++)
                    {
                        var possibleCurrentNodes = currentLayer.Where(node => !node.PreviousNodes.Contains(previousNode)).ToList();
                        if (possibleCurrentNodes.Count <= 0) { break; }

                        var targetCurrentNode = possibleCurrentNodes[currentRandom.Next(possibleCurrentNodes.Count)];

                        targetCurrentNode.PreviousNodes.Add(previousNode);
                        previousNode.NextNodes.Add(targetCurrentNode);
                    }
                }
            }
        }

        return true;
    }
}