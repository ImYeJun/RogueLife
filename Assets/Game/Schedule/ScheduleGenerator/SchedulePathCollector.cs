using System;
using System.Collections.Generic;

public class SchedulePathCollector
{
    private SchedulePathRule rule;
    private ScheduleGenerationContext generationContext;

    public SchedulePathCollector(SchedulePathRule rule, ScheduleGenerationContext generationContext)
    {
        this.rule = rule;
        this.generationContext = generationContext;
    }

    // public void StartCollect(NodeSkeleton startNode)
    // {
    //     var stack = new Stack<(NodeSkeleton node, SchedulePath path)>();

    //     // 시작 노드의 next부터 스택에 push
    //     foreach (var nextNode in startNode.NextNodes)
    //     {
    //         var path = new SchedulePath();
    //         path.ApplyNode(startNode, NodeType.ENTRY);

    //         stack.Push((nextNode, path));
    //     }

    //     while (stack.Count > 0)
    //     {
    //         var (node, path) = stack.Pop();

    //         // cycle 체크
    //         if (path.Contains(node))
    //         {
    //             throw new InvalidOperationException("What?? Node Skeleton has Cycle?!!");
    //         }

    //         // leaf 노드
    //         if (node.NextNodes.Count == 0)
    //         {
    //             path.ApplyNode(node, NodeType.EXIT);

    //             if (rule.IsFinalValid(path))
    //             {
    //                 generationContext.RegisterCompletePath(path);
    //             }
    //             continue;
    //         }

    //         var availableNextNodeTypes = rule.GetAvailableType(path);

    //         // DFS 순서를 재귀와 최대한 맞추려면 역순 push
    //         foreach (var type in availableNextNodeTypes)
    //         {
    //             foreach (var nextNode in node.NextNodes)
    //             {
    //                 var nextPath = path.Clone();
    //                 nextPath.ApplyNode(node, type);

    //                 stack.Push((nextNode, nextPath));
    //             }
    //         }
    //     }
    // }

    public void StartCollect(NodeSkeleton startNode)
    {
        foreach (var nextNode in startNode.NextNodes)
        {
            SchedulePath path = new SchedulePath();
            path.ApplyNode(startNode, NodeType.ENTRY);
            
            Collect(nextNode, path);
        }
    }

    private void Collect(NodeSkeleton node, SchedulePath path)
    {
        if (path.Contains(node))
        {
            throw new InvalidOperationException("What?? Node Skeleton has Cycle?!!");
        }

        if (node.NextNodes.Count == 0)
        {
            path.ApplyNode(node, NodeType.EXIT);

            if (rule.IsFinalValid(path))
            {
                generationContext.RegisterCompletePath(path);
            }
            return;
        }

        var availableNextNodeTypes = rule.GetAvailableType(path);

        foreach (var type in availableNextNodeTypes){
            foreach (var nextNode in node.NextNodes)
            {
                var nextPath = path.Clone();
                nextPath.ApplyNode(node, type);

                Collect(nextNode, nextPath);
            }
        }
    }
}