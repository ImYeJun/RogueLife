using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ScheduleSystem : IChoiceScheduleSystem
{
    private BattleSystem battleSystem;

    private Node currentNode;
    private List<Node> map;
    private ScheduleData currentScheduleData;
    private EnemyDataSlot bossDataSlot;

    private Action onScheduleEnd;

    public ScheduleSystem(BattleSystem battleSystem, Action onScheduleEnd)
    {
        this.battleSystem = battleSystem;
        this.onScheduleEnd = onScheduleEnd;
    }

    public void StartSchdule(ScheduleData scheduleData)
    {
        currentScheduleData = scheduleData;

        ScheduleEntryNode scheduleEntryNode = new ScheduleEntryNode(MoveNode, SettleScheduleSelection);

        bossDataSlot = null;
        currentNode = scheduleEntryNode;
        currentNode.OnEnter();
    }

    public void SettleScheduleSelection(ScheduleData scheduleData)
    {
        //TODO : scheduleData에 따라 맵 생성

        ScheduleExitNode scheduleExitNode = new ScheduleExitNode(MoveNode, EndSchedule);
        map.Add(scheduleExitNode);

        //TODO : currentNode(ScheduleEntryNode) nextNodes 설정
    }

    public void MoveNode(Node nextNode)
    {
        if (!map.Contains(nextNode)) { Debug.LogError("Current Schedule doesn't contain the given node."); }
        
        currentNode = nextNode;
        nextNode.OnEnter();
    }

    public void EndSchedule()
    {
        onScheduleEnd?.Invoke();
    }

    public void SetBoss(EnemyData bossData)
    {
        bossDataSlot.Data = bossData;
    }
}
