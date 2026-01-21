using System.Collections.Generic;
using UnityEngine;

public class ScheduleSystem : MonoBehaviour
{
    private Node currentNode;
    private List<Node> map;
    private ScheduleData currentScheduleData;
    private BattleSystem battleSystem;

    public void StartSchdule()
    {
        ScheduleEntryNode scheduleEntryNode = new ScheduleEntryNode();
        scheduleEntryNode.OnScheduleSettled += SettleScheduleSelection;
        scheduleEntryNode.OnMoveRequest += MoveNode;

        currentNode = scheduleEntryNode;
        currentNode.OnEnter();
    }

    public void SettleScheduleSelection(ScheduleData scheduleData)
    {
        
    }

    public void MoveNode(Node nextNode)
    {
        
    }

    public void EndSchedule()
    {
        
    }
}
