using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

public class FullScheduleGeneratingTest
{
    private int testCount = 1;

    [Test]
    public void GenerateFullScheudle()
    {
        try
        {
            ScheduleSkeletonRule scheduleSkeletonRule = new ScheduleSkeletonRule(
                minLayer : 7,
                maxLayer : 9,
                minNodePerLayer : 3,
                maxNodePerLayer : 5,
                maxNodeLinkCount : 3,
                additionalLinkMultiplierChance : 0.5f);
            SchedulePathRule schedulePathRule = new SchedulePathRule(
                maxBattleSequence : int.MaxValue,
                maxIncidentSequence : int.MaxValue,
                maxTransactionSequence : int.MaxValue,
                minBattleCount : 0,
                maxBattleCount : int.MaxValue,
                minIncidentCount : 0,
                maxIncidentCount : int.MaxValue,
                minTransactionCount : 0,
                maxTransactionCount : int.MaxValue);
            SchedulePathCountRule schedulePathCountRule = new SchedulePathCountRule(
                minCompeletePath : 0,
                maxCompletePath : 10
            );
            ScheduleGenerator generator = new ScheduleGenerator(scheduleSkeletonRule, schedulePathRule, new BattleSystem(new Player()), schedulePathCountRule);

            string mermaidStoringPath = Path.Combine(Application.persistentDataPath, "Schedule Mermaids");
            if (!Directory.Exists(mermaidStoringPath)) { Directory.CreateDirectory(mermaidStoringPath); }
            string folderPath = Path.Combine(mermaidStoringPath, Guid.NewGuid().ToString());
            if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }

            System.Random random = new System.Random();
            for (int i = 0; i < testCount; i++)
            {
                int seed = random.Next();
                Schedule skeleton = generator.GenerateSchedule(new System.Random(seed), new ScheduleData());
                string fileContent = ToMermaid(skeleton.Map, seed, i, scheduleSkeletonRule, schedulePathRule, schedulePathCountRule);

                string filePath = Path.Combine(folderPath, i.ToString()) + ".md";
                File.WriteAllText(filePath, fileContent);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception: {e.Message}");
            Debug.LogError($"StackTrace:{e.StackTrace}");
        }

    }

    private string ToMermaid(
    Dictionary<int, List<Node>> map,
    int seed,
    int attemptIndex,
    ScheduleSkeletonRule scheduleSkeletonRule,
    SchedulePathRule schedulePathRule,
    SchedulePathCountRule schedulePathCountRule
    )
    {
        var sb = new StringBuilder();

        // ---- Markdown 헤더
        sb.AppendLine("# Schedule Skeleton Generation");
        sb.AppendLine();
        sb.AppendLine($"- **Seed**: `{seed}`");
        sb.AppendLine($"- **Attempt**: `{attemptIndex}`");
        sb.AppendLine();
        sb.AppendLine("## Schedule Skeleton Rule");
        sb.AppendLine($"- Layer: {scheduleSkeletonRule.MinLayer} ~ {scheduleSkeletonRule.MaxLayer}");
        sb.AppendLine($"- NodePerLayer: {scheduleSkeletonRule.MinNodePerLayer} ~ {scheduleSkeletonRule.MaxNodePerLayer}");
        sb.AppendLine($"- MaxNodeLinkCount: {scheduleSkeletonRule.MaxNodeLinkCount}");
        sb.AppendLine($"- AdditionalLinkMultiplierChance: {scheduleSkeletonRule.AdditionalLinkMultiplierChance}");
        sb.AppendLine("## Schedule Path Rule");
        sb.AppendLine($"- MaxBattleSequence: {schedulePathRule.MaxBattleSequence}");
        sb.AppendLine($"- MaxIncidentSequence: {schedulePathRule.MaxIncidentSequence}");
        sb.AppendLine($"- MaxTransactionSequence: {schedulePathRule.MaxTransactionSequence}");
        sb.AppendLine($"- BattleCount: {schedulePathRule.MinBattleCount} ~ {schedulePathRule.MaxBattleCount}");
        sb.AppendLine($"- IncidentCount: {schedulePathRule.MinIncidentCount} ~ {schedulePathRule.MaxIncidentCount}");
        sb.AppendLine($"- TransactionCount: {schedulePathRule.MinTransactionCount} ~ {schedulePathRule.MaxTransactionCount}");
        sb.AppendLine("## Schedule Path Count Rule");
        sb.AppendLine($"- CompeletePath: {schedulePathCountRule.MinCompeletePath} ~ {schedulePathCountRule.MaxCompletePath}");
        sb.AppendLine();

        // ---- Mermaid 시작
        sb.AppendLine("```mermaid");
        sb.AppendLine(
            "%%{init: {" +
            "  'flowchart': {" +
            "    'curve': 'linear'," +
            "    'rankSpacing': 220," +
            "    'nodeSpacing': 120" +
            "  }" +
            "}}%%");
        sb.AppendLine("flowchart LR");

        // ---- Node type styles
        sb.AppendLine("classDef entry fill:#c62828,stroke:#ef9a9a,color:#fff;");
        sb.AppendLine("classDef battle   fill:#2e7d32,stroke:#a5d6a7,color:#fff;");
        sb.AppendLine("classDef incident  fill:#1565c0,stroke:#90caf9,color:#fff;");
        sb.AppendLine("classDef transaction   fill:#6a1b9a,stroke:#ce93d8,color:#fff;");
        sb.AppendLine("classDef exit   fill:#000000,stroke:#ff5252,color:#fff;");
        sb.AppendLine("classDef default fill:#424242,stroke:#9e9e9e,color:#fff;");
        sb.AppendLine();

        Dictionary<Node, string> nodeIds = new();
        int nodeCounter = 0;

        foreach (var layer in map.Keys.OrderBy(l => l))
        {
            sb.AppendLine($"    subgraph Layer_{layer}[\"L{layer}\"]");

            foreach (var node in map[layer])
            {
                string nodeId = $"L{layer}_N{nodeCounter++}";
                nodeIds[node] = nodeId;

                string className = GetMermaidClass(node);
                string nodeTypeName = node.GetType().Name;
                sb.AppendLine(
                    $"        {nodeId}[\"L{layer}\\n{nodeTypeName}\"]:::{className}"
                );
            }

            sb.AppendLine("    end");
            sb.AppendLine();
        }

        foreach (var layer in map.Keys.OrderBy(l => l))
        {
            foreach (var node in map[layer])
            {
                foreach (var next in node.NextNodes)
                {
                    sb.AppendLine($"    {nodeIds[node]} --> {nodeIds[next]}");
                }
            }
        }

        sb.AppendLine("```");
        return sb.ToString();
    }


    string GetMermaidClass(Node node)
    {
        return node switch
        {
            ScheduleEntryNode   => "entry",
            BattleNode    => "battle",
            IncidentNode    => "incident",
            TransactionNode    => "transaction",
            ScheduleExitNode    => "exit",
            _                   => "default"
        };
    }

}
