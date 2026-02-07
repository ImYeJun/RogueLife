using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

public class ScheduleSkeletonTest
{
    private int testCount = 10;

    [Test]
    public void TestGeneratingScheduleSkelton()
    {
        try
        {
            ScheduleSkeletonRule rule = new ScheduleSkeletonRule(
                minLayer : 7,
                maxLayer : 9,
                minNodePerLayer : 3,
                maxNodePerLayer : 5,
                maxNodeLinkCount : 3,
                additionalLinkMultiplierChance : 0.5f);
            ScheduleSkeletonGenerator generator = new ScheduleSkeletonGenerator(rule);

            int seed = new System.Random().Next();

            System.Random random = new System.Random(seed);

            string mermaidStoringPath = Path.Combine(Application.persistentDataPath, "Schedule Skeleton Mermaids");
            if (!Directory.Exists(mermaidStoringPath)) { Directory.CreateDirectory(mermaidStoringPath); }
            string folderPath = Path.Combine(Application.persistentDataPath, "Schedule Skeleton Mermaids", Guid.NewGuid().ToString());
            if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }

            for (int i = 0; i < testCount; i++)
            {
                ScheduleSkeleton skeleton = generator.GenerateSkeleton(random);
                string fileContent = ToMermaid(skeleton.LayeredNodes, seed, i, rule);

                string filePath = Path.Combine(folderPath, i.ToString()) + ".md";
                File.WriteAllText(filePath, fileContent);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private string ToMermaid(
    Dictionary<int, HashSet<NodeSkeleton>> layeredNodes,
    int seed,
    int attemptIndex,
    ScheduleSkeletonRule rule)
    {
        var sb = new StringBuilder();

        // ---- Markdown 헤더
        sb.AppendLine("# Schedule Skeleton Generation");
        sb.AppendLine();
        sb.AppendLine($"- **Seed**: `{seed}`");
        sb.AppendLine($"- **Attempt**: `{attemptIndex}`");
        sb.AppendLine();
        sb.AppendLine("## Rule");
        sb.AppendLine($"- Layer: {rule.MinLayer} ~ {rule.MaxLayer}");
        sb.AppendLine($"- NodePerLayer: {rule.MinNodePerLayer} ~ {rule.MaxNodePerLayer}");
        sb.AppendLine($"- MaxNodeLinkCount: {rule.MaxNodeLinkCount}");
        sb.AppendLine($"- AdditionalLinkMultiplierChance: {rule.AdditionalLinkMultiplierChance}");
        sb.AppendLine();

        // ---- Mermaid 시작
        sb.AppendLine("```mermaid");
        sb.AppendLine(
        "%%{init: {" +
        "  'flowchart': {" +
        "    'curve': 'linear'," +
        "    'rankSpacing': 220," +   // 레이어 간 간격 (핵심)
        "    'nodeSpacing': 120" +   // 같은 레이어 내 간격
        "  }" +
        "}}%%");
        sb.AppendLine("flowchart LR");

        // ---- Node ID 매핑
        Dictionary<NodeSkeleton, string> nodeIds = new();
        int nodeCounter = 0;

        // ---- 레이어별 subgraph + 노드 선언
        foreach (var layer in layeredNodes.Keys.OrderBy(l => l))
        {
            sb.AppendLine($"    subgraph Layer_{layer}[\"L{layer}\"]");

            foreach (var node in layeredNodes[layer])
            {
                string nodeId = $"L{layer}_N{nodeCounter++}";
                nodeIds[node] = nodeId;

                sb.AppendLine($"        {nodeId}[\"L{layer}\"]");
            }

            sb.AppendLine("    end");
            sb.AppendLine();
        }

        // ---- 링크 선언 (직접 연결만)
        foreach (var layer in layeredNodes.Keys.OrderBy(l => l))
        {
            foreach (var node in layeredNodes[layer])
            {
                foreach (var next in node.NextNodes)
                {
                    sb.AppendLine($"    {nodeIds[node]} --> {nodeIds[next]}");
                }
            }
        }

        // ---- Mermaid 종료
        sb.AppendLine("```");

        return sb.ToString();
    }


        // private string ToMermaid(
        // Dictionary<int, HashSet<NodeSkeleton>> layeredNodes,
        // int seed,
        // int attemptIndex,
        // ScheduleSkeletonRule rule)
        // {
        //     var sb = new StringBuilder();

        //     // ---- Markdown 헤더
        //     sb.AppendLine($"# Schedule Skeleton Generation");
        //     sb.AppendLine();
        //     sb.AppendLine($"- **Seed**: `{seed}`");
        //     sb.AppendLine($"- **Attempt**: `{attemptIndex}`");
        //     sb.AppendLine();
        //     sb.AppendLine("## Rule");
        //     sb.AppendLine($"- Layer: {rule.MinLayer} ~ {rule.MaxLayer}");
        //     sb.AppendLine($"- NodePerLayer: {rule.MinNodePerLayer} ~ {rule.MaxNodePerLayer}");
        //     sb.AppendLine($"- MaxNodeLinkCount: {rule.MaxNodeLinkCount}");
        //     sb.AppendLine($"- AdditionalLinkMultiplierChance: {rule.AdditionalLinkMultiplierChance}");
        //     sb.AppendLine();

        //     // ---- Mermaid fenced block 시작
        //     sb.AppendLine("```mermaid");
        //     sb.AppendLine("%%{init: {'flowchart': {'curve': 'linear'}}}%%");
        //     sb.AppendLine("flowchart TD");


        //     // // ---- Mermaid 주석 (렌더엔 영향 없음)
        //     // sb.AppendLine($"%% Seed: {seed}");
        //     // sb.AppendLine($"%% Attempt: {attemptIndex}");
        //     // sb.AppendLine($"%% Rule:");
        //     // sb.AppendLine($"%%  - Layer: {rule.MinLayer} ~ {rule.MaxLayer}");
        //     // sb.AppendLine($"%%  - NodePerLayer: {rule.MinNodePerLayer} ~ {rule.MaxNodePerLayer}");
        //     // sb.AppendLine($"%%  - MaxNodeLinkCount: {rule.MaxNodeLinkCount}");
        //     // sb.AppendLine($"%%  - AdditionalLinkMultiplierChance: {rule.AdditionalLinkMultiplierChance}");
        //     // sb.AppendLine();

        //     // // ---- 메타 정보 subgraph
        //     // sb.AppendLine("    subgraph META[\"Generation Info\"]");
        //     // sb.AppendLine($"        META_SEED[\"Seed: {seed}\"]");
        //     // sb.AppendLine($"        META_TRY[\"Attempt: {attemptIndex}\"]");
        //     // sb.AppendLine(
        //     //     $"        META_RULE[\"Rule\\n" +
        //     //     $"Layers: {rule.MinLayer}~{rule.MaxLayer}\\n" +
        //     //     $"Nodes: {rule.MinNodePerLayer}~{rule.MaxNodePerLayer}\\n" +
        //     //     $"MaxLinks: {rule.MaxNodeLinkCount}\\n" +
        //     //     $"Chance: {rule.AdditionalLinkMultiplierChance}\"]");
        //     // sb.AppendLine("    end");
        //     // sb.AppendLine();

        //     // ---- Node ID 매핑
        //     Dictionary<NodeSkeleton, string> nodeIds = new();
        //     int nodeCounter = 0;

        //     // ---- 노드 선언
        //     foreach (var layer in layeredNodes.Keys.OrderBy(l => l))
        //     {
        //         foreach (var node in layeredNodes[layer])
        //         {
        //             string nodeId = $"L{layer}_N{nodeCounter++}";
        //             nodeIds[node] = nodeId;

        //             sb.AppendLine($"    {nodeId}[\"L{layer}\"]");
        //         }
        //     }

        //     sb.AppendLine();

        //     // ---- 링크 선언
        //     foreach (var layer in layeredNodes.Keys.OrderBy(l => l))
        //     {
        //         foreach (var node in layeredNodes[layer])
        //         {
        //             foreach (var next in node.NextNodes)
        //             {
        //                 sb.AppendLine($"    {nodeIds[node]} --> {nodeIds[next]}");
        //             }
        //         }
        //     }

        //     // ---- Mermaid fenced block 종료
        //     sb.AppendLine("```");

        //     return sb.ToString();
        // }

}
