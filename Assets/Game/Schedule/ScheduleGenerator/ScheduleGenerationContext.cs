using System;
using System.Collections.Generic;

public class ScheduleGenerationContext
{
    private List<SchedulePath> completePaths = new List<SchedulePath>();
    private Dictionary<NodeSkeleton, List<SchedulePath>> passingByPathsOnNode = new Dictionary<NodeSkeleton, List<SchedulePath>>();

    public int CompletePathCount { get => completePaths.Count; }

    public void RegisterCompletePath(SchedulePath path)
    {
        completePaths.Add(path);

        foreach (var visitedNode in path.VisitedNodes)
        {
            if (!passingByPathsOnNode.ContainsKey(visitedNode.Key))
            {
                passingByPathsOnNode.Add(visitedNode.Key, new List<SchedulePath>());
            }

            passingByPathsOnNode[visitedNode.Key].Add(path);
        }
    }

    public void ResetContext()
    {
        completePaths.Clear();
        passingByPathsOnNode.Clear();
    }
}