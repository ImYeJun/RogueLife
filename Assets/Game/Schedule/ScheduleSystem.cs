using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ScheduleSystem : MonoBehaviour
{
    private Node currentNode;
    private List<Node> map;
    private ScheduleData currentScheduleData;
    private BattleSystem battleSystem;
    private int finishedSchedulesCount = 0;

    public void StartSchdule()
    {
        ScheduleEntryNode scheduleEntryNode = new ScheduleEntryNode(MoveNode, SettleScheduleSelection);

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
        //TODO : 현재 일정 종료 처리

        if (++finishedSchedulesCount == 3)
        {
            //TODO : 일기 보러가기 기능 구현
        }
        else
        {
            StartSchdule();
        }
    }
}
